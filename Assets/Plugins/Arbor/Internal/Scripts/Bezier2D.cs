//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 2次元の3次ベジェを扱うクラス
	/// </summary>
#else
	/// <summary>
	/// Class to handle the two-dimensional cubic Bezier
	/// </summary>
#endif
	[System.Serializable]
	public sealed class Bezier2D : System.IEquatable<Bezier2D>
	{
		const int kCalculateLengthSplitNum = 16;

		[SerializeField] private Vector2 _StartPosition;

		[SerializeField] private Vector2 _StartControl;

		[SerializeField] private Vector2 _EndPosition;

		[SerializeField] private Vector2 _EndControl;

		float _Length = 0.0f;
		float[] _SplitLength;
		bool _IsLengthDirty = false;
		Vector2 _LastStartPosition;
		Vector2 _LastStartControl;
		Vector2 _LastEndPosition;
		Vector2 _LastEndControl;

#if ARBOR_DOC_JA
		/// <summary>
		/// 始点
		/// </summary>
#else
		/// <summary>
		/// Starting point
		/// </summary>
#endif
		public Vector2 startPosition
		{
			get
			{
				return _StartPosition;
			}
			set
			{
				if (_StartPosition != value)
				{
					_StartPosition = value;
					_IsLengthDirty = true;
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 始点のコントロール点
		/// </summary>
#else
		/// <summary>
		/// Control point of the starting point
		/// </summary>
#endif
		public Vector2 startControl
		{
			get
			{
				return _StartControl;
			}
			set
			{
				if (_StartControl != value)
				{
					_StartControl = value;
					_IsLengthDirty = true;
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 終点
		/// </summary>
#else
		/// <summary>
		/// End point
		/// </summary>
#endif
		public Vector2 endPosition
		{
			get
			{
				return _EndPosition;
			}
			set
			{
				if (_EndPosition != value)
				{
					_EndPosition = value;
					_IsLengthDirty = true;
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 終点のコントロール点
		/// </summary>
#else
		/// <summary>
		/// Control point of the end point
		/// </summary>
#endif
		public Vector2 endControl
		{
			get
			{
				return _EndControl;
			}
			set
			{
				if (_EndControl != value)
				{
					_EndControl = value;
					_IsLengthDirty = true;
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 長さ
		/// </summary>
#else
		/// <summary>
		/// Length
		/// </summary>
#endif
		public float length
		{
			get
			{
				CalculateLength();
				return _Length;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 変更されたかどうか
		/// </summary>
#else
		/// <summary>
		/// Whether it has been changed
		/// </summary>
#endif
		public bool isChanged
		{
			get
			{
				return (_IsLengthDirty ||
					_StartPosition != _LastStartPosition ||
					_StartControl != _LastStartControl ||
					_EndPosition != _LastEndPosition ||
					_EndControl != _LastEndControl);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bezier2Dを作成する。
		/// </summary>
#else
		/// <summary>
		/// new Bezier2D.
		/// </summary>
#endif
		public Bezier2D()
		{
			_StartPosition = Vector2.zero;
			_StartControl = Vector2.zero;
			_EndPosition = Vector2.zero;
			_EndControl = Vector2.zero;
			_IsLengthDirty = true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bezier2Dを作成する。
		/// </summary>
		/// <param name="startPosition">始点</param>
		/// <param name="startControl">始点のコントロール点</param>
		/// <param name="endPosition">終点</param>
		/// <param name="endControl">終点のコントロール点</param>
#else
		/// <summary>
		/// new Bezier2D.
		/// </summary>
		/// <param name="startPosition">Starting point</param>
		/// <param name="startControl">Control point of the end point</param>
		/// <param name="endPosition">End point</param>
		/// <param name="endControl">Control point of the end point</param>
#endif
		public Bezier2D(Vector2 startPosition, Vector2 startControl, Vector2 endPosition, Vector2 endControl)
		{
			_StartPosition = startPosition;
			_StartControl = startControl;
			_EndPosition = endPosition;
			_EndControl = endControl;
			_IsLengthDirty = true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bezier2Dを作成する。
		/// </summary>
		/// <param name="bezier">コピー元のBezier2D</param>
#else
		/// <summary>
		/// new Bezier2D.
		/// </summary>
		/// <param name="bezier">Copy source Bezier2D</param>
#endif
		public Bezier2D(Bezier2D bezier)
		{
			_StartPosition = bezier.startPosition;
			_StartControl = bezier.startControl;
			_EndPosition = bezier.endPosition;
			_EndControl = bezier.endControl;
			_IsLengthDirty = true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始点を設定する。
		/// </summary>
		/// <param name="position">位置</param>
		/// <param name="control">制御点</param>
		/// <returns>変更した場合はtrue。</returns>
#else
		/// <summary>
		/// Set Start point.
		/// </summary>
		/// <param name="position">Position</param>
		/// <param name="control">Control point</param>
		/// <returns>true if changed.</returns>
#endif
		public bool SetStartPoint(Vector2 position, Vector2 control)
		{
			if (_StartPosition != position || _StartControl != control)
			{
				_StartPosition = position;
				_StartControl = control;
				_IsLengthDirty = true;
				return true;
			}
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 終点を設定する。
		/// </summary>
		/// <param name="position">位置</param>
		/// <param name="control">制御点</param>
		/// <returns>変更した場合はtrue。</returns>
#else
		/// <summary>
		/// Set End point.
		/// </summary>
		/// <param name="position">Position</param>
		/// <param name="control">Control point</param>
		/// <returns>true if changed.</returns>
#endif
		public bool SetEndPoint(Vector2 position, Vector2 control)
		{
			if (_EndPosition != position || _EndControl != control)
			{
				_EndPosition = position;
				_EndControl = control;
				_IsLengthDirty = true;
				return true;
			}
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ベジェ曲線上の位置を求める。
		/// </summary>
		/// <param name="t">0から1の値</param>
		/// <returns>ベジェ曲線上の位置を返す。</returns>
#else
		/// <summary>
		/// Get a position on the Bezier curve.
		/// </summary>
		/// <param name="t">0-1 of value</param>
		/// <returns>Returns the position on the Bezier curve.</returns>
#endif
		public Vector2 GetPoint(float t)
		{
			return GetPoint(_StartPosition, _StartControl, _EndPosition, _EndControl, t);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ベジェ曲線上の接線を求める。
		/// </summary>
		/// <param name="t">0から1の値</param>
		/// <returns>ベジェ曲線上の接線を返す。</returns>
#else
		/// <summary>
		/// Get a tangent on the Bezier curve.
		/// </summary>
		/// <param name="t">0-1 of value</param>
		/// <returns>Returns the tangent on the Bezier curve.</returns>
#endif
		public Vector2 GetTangent(float t)
		{
			return GetTangent(_StartPosition, _StartControl, _EndPosition, _EndControl, t);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ベジェ曲線上の最近接点を求める。
		/// </summary>
		/// <param name="pos">位置</param>
		/// <param name="thresholdT">最近接パラメータの閾値</param>
		/// <returns>最近接点</returns>
#else
		/// <summary>
		/// Get a closest point on the Bezier curve.
		/// </summary>
		/// <param name="pos">position</param>
		/// <param name="thresholdT">threshold of closest param.</param>
		/// <returns>Closest point</returns>
#endif
		public Vector2 GetClosestPoint(Vector2 pos, float thresholdT = 0.001f)
		{
			return GetPoint(GetClosestParam(pos, thresholdT));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ベジェ曲線上の最近接パラメータを求める。
		/// </summary>
		/// <param name="pos">位置</param>
		/// <param name="thresholdT">最近接パラメータの閾値</param>
		/// <returns>最近接パラメータ</returns>
#else
		/// <summary>
		/// Get a closest point on the Bezier curve.
		/// </summary>
		/// <param name="pos">position</param>
		/// <param name="thresholdT">threshold of closest param.</param>
		/// <returns>Closest param</returns>
#endif
		public float GetClosestParam(Vector2 pos, float thresholdT = 0.001f)
		{
			float beginT = 0.0f;
			float endT = 1.0f;

			float midT = (beginT + endT) * 0.5f;

			while ((endT - beginT) >= thresholdT)
			{
				float a = (beginT + midT) * 0.5f;
				float b = (endT + midT) * 0.5f;

				float distanceA = (GetPoint(a) - pos).sqrMagnitude;
				float distanceB = (GetPoint(b) - pos).sqrMagnitude;

				if (distanceA < distanceB)
				{
					endT = midT;
				}
				else
				{
					beginT = midT;
				}

				midT = (beginT + endT) * 0.5f;
			}

			return midT;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ベジェ曲線上の位置を求める。
		/// </summary>
		/// <param name="startPosition">始点</param>
		/// <param name="startControl">始点のコントロール点</param>
		/// <param name="endPosition">終点</param>
		/// <param name="endControl">終点のコントロール点</param>
		/// <param name="t">0から1の値</param>
		/// <returns>ベジェ曲線上の位置を返す。</returns>
#else
		/// <summary>
		/// Get a position on the Bezier curve.
		/// </summary>
		/// <param name="startPosition">Starting point</param>
		/// <param name="startControl">Control point of the starting point</param>
		/// <param name="endPosition">End point</param>
		/// <param name="endControl">Control point of the end point</param>
		/// <param name="t">0-1 of value</param>
		/// <returns>Returns the position on the Bezier curve.</returns>
#endif
		public static Vector2 GetPoint(Vector2 startPosition, Vector2 startControl, Vector2 endPosition, Vector2 endControl, float t)
		{
			float t2 = t * t;
			float t3 = t * t * t;
			float t_1 = 1.0f - t;
			float t_12 = t_1 * t_1;
			float t_13 = t_1 * t_1 * t_1;
			return t_13 * startPosition + 3 * t_12 * t * startControl + 3 * t_1 * t2 * endControl + t3 * endPosition;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ベジェ曲線上の接線を求める。
		/// </summary>
		/// <param name="startPosition">始点</param>
		/// <param name="startControl">始点のコントロール点</param>
		/// <param name="endPosition">終点</param>
		/// <param name="endControl">終点のコントロール点</param>
		/// <param name="t">0から1の値</param>
		/// <returns>ベジェ曲線上の接線を返す。</returns>
#else
		/// <summary>
		/// Get a tangent on the Bezier curve.
		/// </summary>
		/// <param name="startPosition">Starting point</param>
		/// <param name="startControl">Control point of the starting point</param>
		/// <param name="endPosition">End point</param>
		/// <param name="endControl">Control point of the end point</param>
		/// <param name="t">0-1 of value</param>
		/// <returns>Returns the tangent on the Bezier curve.</returns>
#endif
		public static Vector2 GetTangent(Vector2 startPosition, Vector2 startControl, Vector2 endPosition, Vector2 endControl, float t)
		{
			float t2 = t * t;
			float t_1 = 1.0f - t;
			float t_12 = t_1 * t_1;
			return (-3 * t_12 * startPosition + 3 * t_12 * startControl - 6 * t * t_1 * startControl - 3 * t2 * endControl + 6 * t * t_1 * endControl + 3 * t2 * endPosition).normalized;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ベジェ曲線のバウンディングボックスを求める。
		/// </summary>
		/// <param name="startPosition">始点</param>
		/// <param name="startControl">始点のコントロール点</param>
		/// <param name="endPosition">終点</param>
		/// <param name="endControl">終点のコントロール点</param>
		/// <returns>ベジェ曲線のバウンディングボックスを返す。</returns>
#else
		/// <summary>
		/// Get the bounding box of a Bezier curve.
		/// </summary>
		/// <param name="startPosition">Starting point</param>
		/// <param name="startControl">Control point of the starting point</param>
		/// <param name="endPosition">End point</param>
		/// <param name="endControl">Control point of the end point</param>
		/// <returns>Returns the Bezier curve bounding box.</returns>
#endif
		public static Rect GetBoundingBox(Vector2 startPosition, Vector2 startControl, Vector2 endPosition, Vector2 endControl)
		{
			Vector2 min = Vector2.Min(startPosition, endPosition);
			Vector2 max = Vector2.Max(startPosition, endPosition);

			Vector2 i = startControl - startPosition;
			Vector2 j = endControl - startControl;
			Vector2 k = endPosition - endControl;

			Vector2 a = 3f * i - 6f * j + 3f * k;
			Vector2 b = 6f * j - 6f * i;
			Vector2 c = 3f * i;

			for (int index = 0; index < 2; index++)
			{
				float sqrtPart = b[index] * b[index] - 4f * a[index] * c[index];
				if (sqrtPart < 0f)
				{
					continue;
				}

				float div2d = 1f / (2f * a[index]);
				float part = Mathf.Sqrt(sqrtPart);

				float t1 = (-b[index] + part) * div2d;
				if (t1 >= 0 && t1 <= 1f)
				{
					float s1 = GetBezierValueForT(t1, startPosition[index], startControl[index], endControl[index], endPosition[index]);
					min[index] = Mathf.Min(min[index], s1);
					max[index] = Mathf.Max(max[index], s1);
				}

				float t2 = (-b[index] - part) * div2d ;
				if (t2 >= 0 && t2 <= 1f)
				{
					float s2 = GetBezierValueForT(t2, startPosition[index], startControl[index], endControl[index], endPosition[index]);
					min[index] = Mathf.Min(min[index], s2);
					max[index] = Mathf.Max(max[index], s2);
				}
			}

			return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
		}

		private static float GetBezierValueForT(float t, float p0, float p1, float p2, float p3)
		{
			float t2 = t * t;
			float t3 = t2 * t;
			float t_1 = 1.0f - t;
			float t_12 = t_1 * t_1;
			float t_13 = t_12 * t_1;

			return t_13 * p0
				+ 3f * t_12 * t * p1
				+ 3f * t_1 * t2 * p2
				+ t3 * p3;
		}
	
		void CalculateLength()
		{
			if (_SplitLength == null || _SplitLength.Length == 0 || isChanged)
			{
				_Length = 0;
				if (_SplitLength == null || _SplitLength.Length == 0)
				{
					_SplitLength = new float[kCalculateLengthSplitNum + 1];
				}
				_SplitLength[0] = 0.0f;
				Vector2 oldPos = _StartPosition;

				for (int i = 1; i <= kCalculateLengthSplitNum; i++)
				{
					Vector2 pos = GetPoint(i / (float)kCalculateLengthSplitNum);
					_Length += (pos - oldPos).magnitude;
					_SplitLength[i] = _Length;
					oldPos = pos;
				}

				for (int i = 0; i <= kCalculateLengthSplitNum; i++)
				{
					_SplitLength[i] /= _Length;
				}

				_IsLengthDirty = false;
				_LastStartPosition = _StartPosition;
				_LastStartControl = _StartControl;
				_LastEndPosition = _EndPosition;
				_LastEndControl = _EndControl;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 線形の補間値tからベジェ曲線の通常の補間値を返す。
		/// </summary>
		/// <param name="t">0から1の値</param>
		/// <returns>ベジェ曲線の通常の補間値を返す。</returns>
#else
		/// <summary>
		/// Returns normal interpolation value of Bezier curve from linear interpolation value t.
		/// </summary>
		/// <param name="t">0-1 of value</param>
		/// <returns>Returns normal interpolation value of Bezier curve.</returns>
#endif
		public float LinearToInterpolationParam(float t)
		{
			CalculateLength();

			for (int k = 0; k < kCalculateLengthSplitNum; k++)
			{
				float current = _SplitLength[k];
				float next = _SplitLength[k + 1];
				if (current <= t && t <= next)
				{
					float length = next - current;
					if (Mathf.Approximately(length, 0f))
					{
						continue;
					}

					float x = (t - current) / length;
					x = (k * (1 - x) + (1 + k) * x) / (float)kCalculateLengthSplitNum;
					return x;
				}
			}

			return 0.0f;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ベジェ曲線上の位置を線形に求める。
		/// </summary>
		/// <param name="t">0から1の値</param>
		/// <returns>線形に求めたベジェ曲線上の位置を返す。</returns>
#else
		/// <summary>
		/// Get a position on the Bezier curve linear.
		/// </summary>
		/// <param name="t">0-1 of value</param>
		/// <returns>Returns the position on the Bezier curve linear</returns>
#endif
		public Vector2 GetLinearPoint(float t)
		{
			return GetPoint(LinearToInterpolationParam(t));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bezier2Dと等しいかを返す。
		/// </summary>
		/// <param name="bezier">Bezier2Dの値</param>
		/// <returns>等しい場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether this is equal to Bezier2D.
		/// </summary>
		/// <param name="bezier">Value of Bezier2D</param>
		/// <returns>Returns true if they are equal.</returns>
#endif
		public bool Equals(Bezier2D bezier)
		{
			if ((object)bezier == null)
			{
				return false;
			}

			return _StartPosition == bezier._StartPosition &&
					_StartControl == bezier._StartControl &&
					_EndPosition == bezier._EndPosition &&
					_EndControl == bezier._EndControl;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// objectと等しいかを返す。
		/// </summary>
		/// <param name="obj">objectの値</param>
		/// <returns>等しい場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether this is equal to object.
		/// </summary>
		/// <param name="obj">object value</param>
		/// <returns>Returns true if they are equal.</returns>
#endif
		public override bool Equals(object obj)
		{
			return Equals(obj as Bezier2D);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ハッシュ値を取得する。
		/// </summary>
		/// <returns>ハッシュ値</returns>
#else
		/// <summary>
		/// Get a hash code.
		/// </summary>
		/// <returns>Hash code</returns>
#endif
		public override int GetHashCode()
		{
			return _StartPosition.GetHashCode() ^ _StartControl.GetHashCode() ^ _EndPosition.GetHashCode() ^ _EndControl.GetHashCode();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を文字列形式に変換する。
		/// </summary>
		/// <returns>変換した文字列</returns>
#else
		/// <summary>
		/// Convert value to string format.
		/// </summary>
		/// <returns>Converted string</returns>
#endif
		public override string ToString()
		{
			return string.Format("(startPosition {0}, startControl {1}, endPosition {2}, endControl {3}",startPosition.ToString(), startControl.ToString(), endPosition.ToString(), endControl.ToString());
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bezier2Dが等しいかを返す。
		/// </summary>
		/// <param name="x">Bezier2D</param>
		/// <param name="y">Bezier2D</param>
		/// <returns>等しい場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether Bezier2D is equal.
		/// </summary>
		/// <param name="x">Bezier2D</param>
		/// <param name="y">Bezier2D</param>
		/// <returns>Returns true if they are equal.</returns>
#endif
		public static bool operator ==(Bezier2D x, Bezier2D y)
		{
			if (ReferenceEquals(x, y))
			{
				return true;
			}

			if ((object)x == null || (object)y == null)
			{
				return false;
			}

			return x.Equals(y);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bezier2Dが等しくないかを返す。
		/// </summary>
		/// <param name="x">Bezier2D</param>
		/// <param name="y">Bezier2D</param>
		/// <returns>等しくない場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether Bezier2D are not equal.
		/// </summary>
		/// <param name="x">Bezier2D</param>
		/// <param name="y">Bezier2D</param>
		/// <returns>Returns true if it is not equal.</returns>
#endif
		public static bool operator !=(Bezier2D x, Bezier2D y)
		{
			return !(x == y);
		}
	}
}
