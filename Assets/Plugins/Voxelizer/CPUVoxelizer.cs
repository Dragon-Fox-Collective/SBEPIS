using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace VoxelSystem
{
	public class CPUVoxelizer
	{
		public class Triangle
		{
			public Vector3 a, b, c;
			public Bounds bounds;
			public bool frontFacing;
			
			public Triangle (Vector3 a, Vector3 b, Vector3 c, Vector3 dir)
			{
				this.a = a;
				this.b = b;
				this.c = c;
				
				var cross = Vector3.Cross(b - a, c - a);
				this.frontFacing = (Vector3.Dot(cross, dir) <= 0f);
				
				var min = Vector3.Min(Vector3.Min(a, b), c);
				var max = Vector3.Max(Vector3.Max(a, b), c);
				bounds.SetMinMax(min, max);
			}
			
			public Vector2 GetUV(Vector3 p, Vector2 uva, Vector2 uvb, Vector2 uvc)
			{
				float u, v, w;
				Barycentric(p, out u, out v, out w);
				return uva * u + uvb * v + uvc * w;
			}
			
			// https://gamedev.stackexchange.com/questions/23743/whats-the-most-efficient-way-to-find-barycentric-coordinates
			public void Barycentric(Vector3 p, out float u, out float v, out float w)
			{
				Vector3 v0 = b - a, v1 = c - a, v2 = p - a;
				float d00 = Vector3.Dot(v0, v0);
				float d01 = Vector3.Dot(v0, v1);
				float d11 = Vector3.Dot(v1, v1);
				float d20 = Vector3.Dot(v2, v0);
				float d21 = Vector3.Dot(v2, v1);
				float denom = 1f / (d00 * d11 - d01 * d01);
				v = (d11 * d20 - d01 * d21) * denom;
				w = (d00 * d21 - d01 * d20) * denom;
				u = 1.0f - v - w;
			}
		}
		
		// http://blog.wolfire.com/2009/11/Triangle-mesh-voxelization
		public static void Voxelize (Mesh mesh, int resolution, out List<Voxel_t> voxels, out float unit, out Voxel_t[,,] volume, out Vector3 start)
		{
			mesh.RecalculateBounds();
			
			var bounds = mesh.bounds;
			bounds.extents += Vector3.one * 0.1f; // Rounding error
			float maxLength = Mathf.Max(bounds.size.x, Mathf.Max(bounds.size.y, bounds.size.z));
			unit = maxLength / resolution;
			var hunit = unit * 0.5f;
			
			start = bounds.min - 3 * new Vector3(hunit, hunit, hunit);
			var end = bounds.max + 3 * new Vector3(hunit, hunit, hunit);
			var size = end - start;
			
			var width = Mathf.CeilToInt(size.x / unit);
			var height = Mathf.CeilToInt(size.y / unit);
			var depth = Mathf.CeilToInt(size.z / unit);
			
			volume = new Voxel_t[width, height, depth];
			var boxes = new Bounds[width, height, depth];
			var voxelSize = Vector3.one * unit;
			for(int x = 0; x < width; x++)
			{
				for(int y = 0; y < height; y++)
				{
					for(int z = 0; z < depth; z++)
					{
						var p = new Vector3(x, y, z) * unit + start;
						var aabb = new Bounds(p, voxelSize);
						boxes[x, y, z] = aabb;
						volume[x, y, z].position = aabb.center;
					}
				}
			}
			
			// build triangles
			var vertices = mesh.vertices;
			var uvs = mesh.uv;
			var uv00 = Vector2.zero;
			var indices = mesh.triangles;
			var direction = Vector3.forward;

			for(int i = 0, n = indices.Length; i < n; i += 3) {
				var tri = new Triangle(
					vertices[indices[i]],
					vertices[indices[i + 1]],
					vertices[indices[i + 2]],
					direction
				);

				Vector2 uva, uvb, uvc;
				if(uvs.Length > 0) {
					uva = uvs[indices[i]];
					uvb = uvs[indices[i + 1]];
					uvc = uvs[indices[i + 2]];
				} else {
					uva = uvb = uvc = uv00;
				}

				var min = tri.bounds.min - start;
				var max = tri.bounds.max - start;
				int iminX = Mathf.RoundToInt(min.x / unit), iminY = Mathf.RoundToInt(min.y / unit), iminZ = Mathf.RoundToInt(min.z / unit);
				int imaxX = Mathf.RoundToInt(max.x / unit), imaxY = Mathf.RoundToInt(max.y / unit), imaxZ = Mathf.RoundToInt(max.z / unit);
				// int iminX = Mathf.FloorToInt(min.x / unit), iminY = Mathf.FloorToInt(min.y / unit), iminZ = Mathf.FloorToInt(min.z / unit);
				// int imaxX = Mathf.CeilToInt(max.x / unit), imaxY = Mathf.CeilToInt(max.y / unit), imaxZ = Mathf.CeilToInt(max.z / unit);

				iminX = Mathf.Clamp(iminX, 0, width - 1);
				iminY = Mathf.Clamp(iminY, 0, height - 1);
				iminZ = Mathf.Clamp(iminZ, 0, depth - 1);
				imaxX = Mathf.Clamp(imaxX, 0, width - 1);
				imaxY = Mathf.Clamp(imaxY, 0, height - 1);
				imaxZ = Mathf.Clamp(imaxZ, 0, depth - 1);

				// Debug.Log((iminX + "," + iminY + "," + iminZ) + " ~ " + (imaxX + "," + imaxY + "," + imaxZ));

				uint front = (uint)(tri.frontFacing ? 1 : 0);

				for(int x = iminX; x <= imaxX; x++) {
					for(int y = iminY; y <= imaxY; y++) {
						for(int z = iminZ; z <= imaxZ; z++) {
							if(Intersects(tri, boxes[x, y, z])) {
								var voxel = volume[x, y, z];
								voxel.uv = tri.GetUV(voxel.position, uva, uvb, uvc);
								if((voxel.fill & 1) == 0)
								{
									voxel.front = front;
								} else
								{
									voxel.front &= front;
								}
								voxel.fill |= 1;
								volume[x, y, z] = voxel;
							}
						}
					}
				}
			}
			
			for(int x = 0; x < width; x++)
			{
				for(int y = 0; y < height; y++)
				{
					for(int z = 0; z < depth; z++)
					{
						if (volume[x, y, z].IsEmpty()) continue;

						int ifront = z;

						Vector2 uv = Vector2.zero;
						for(; ifront < depth; ifront++) {
							if(!volume[x, y, ifront].IsFrontFace()) {
								break;
							}
							uv = volume[x, y, ifront].uv;
						}

						if(ifront >= depth) break;

						var iback = ifront;

						// step forward to cavity
						for (; iback < depth && volume[x, y, iback].IsEmpty(); iback++) {}

						if (iback >= depth) break;

						// check if iback is back voxel
						if(volume[x, y, iback].IsBackFace()) {
							// step forward to back face
							for (; iback < depth && volume[x, y, iback].IsBackFace(); iback++) {}
						}

						// fill from ifront to iback
						for(int z2 = ifront; z2 < iback; z2++)
						{
							var p = boxes[x, y, z2].center;
							var voxel = volume[x, y, z2];
							voxel.position = p;
							voxel.uv = uv;
							voxel.fill = 1;
							volume[x, y, z2] = voxel;
						}

						z = iback;
					}
				}
			}
			
			voxels = new List<Voxel_t>();
			for(int x = 0; x < width; x++)
			for(int y = 0; y < height; y++)
			for(int z = 0; z < depth; z++)
				if (!volume[x, y, z].IsEmpty())
					voxels.Add(volume[x, y, z]);
		}
		
		public static bool Intersects(Triangle tri, Bounds aabb)
		{
			// Vertex positions relative to bounds center
			Vector3 v0 = tri.a - aabb.center;
			Vector3 v1 = tri.b - aabb.center;
			Vector3 v2 = tri.c - aabb.center;
			
			// Triangle edge vectors
			Vector3 e0 = v1 - v0;
			Vector3 e1 = v2 - v1;
			Vector3 e2 = v0 - v2;
			
			// Normals between triangle edges and aabb face normals
			Vector3 a00 = Vector3.Cross(e0, Vector3.right);
			Vector3 a01 = Vector3.Cross(e1, Vector3.right);
			Vector3 a02 = Vector3.Cross(e2, Vector3.right);
			Vector3 a10 = Vector3.Cross(e0, Vector3.up);
			Vector3 a11 = Vector3.Cross(e1, Vector3.up);
			Vector3 a12 = Vector3.Cross(e2, Vector3.up);
			Vector3 a20 = Vector3.Cross(e0, Vector3.forward);
			Vector3 a21 = Vector3.Cross(e1, Vector3.forward);
			Vector3 a22 = Vector3.Cross(e2, Vector3.forward);
			
			Vector3[] axes =
			{
				a00, a01, a02,
				a10, a11, a12,
				a20, a21, a22,
				Vector3.right, Vector3.up, Vector3.forward,
				Vector3.Cross(e1, e0)
			};
			
			bool TraingleIntersectsAABBWhenProjectedOnto(Vector3 axis)
			{
				// Projected vertices
				float p0 = Vector3.Dot(v0, axis);
				float p1 = Vector3.Dot(v1, axis);
				float p2 = Vector3.Dot(v2, axis);
				
				// Projected vertex line
				float maxP = Mathf.Max(p0, p1, p2);
				float minP = Mathf.Min(p0, p1, p2);
				
				// Projected AABB
				float r = aabb.extents.x * Mathf.Abs(Vector3.Dot(Vector3.right, axis)) +
				          aabb.extents.y * Mathf.Abs(Vector3.Dot(Vector3.up, axis)) +
				          aabb.extents.z * Mathf.Abs(Vector3.Dot(Vector3.forward, axis));
				
				// Projected AABB line would be from -r to r
				
				return Mathf.Max(-maxP, minP) <= r;
			}
			
			return axes.All(TraingleIntersectsAABBWhenProjectedOnto);
		}
	}
}


