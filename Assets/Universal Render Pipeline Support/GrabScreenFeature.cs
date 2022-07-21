using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;

public class GrabScreenFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public string TextureName = "_GrabPassTransparent";
        public LayerMask LayerMask;
        public RenderPassEvent RenderPassEvent;
        public int Offset;
        public Material BlitMaterial;
    }

    class GrabPass : ScriptableRenderPass
    {
        RenderTargetHandle tempColorTarget;
        Settings settings;

        public GrabPass(Settings s)
        {
            settings = s;
            renderPassEvent = settings.RenderPassEvent + settings.Offset;
            tempColorTarget.Init(settings.TextureName);
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            var descriptor = cameraTextureDescriptor;
            descriptor.msaaSamples = 1;
            descriptor.depthBufferBits = 0;

            cmd.GetTemporaryRT(tempColorTarget.id, descriptor);
            cmd.SetGlobalTexture(settings.TextureName, tempColorTarget.Identifier());

            ConfigureTarget(tempColorTarget.Identifier());
            ConfigureClear(ClearFlag.Color, Color.black);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            if ((int)settings.RenderPassEvent >= (int)RenderPassEvent.BeforeRenderingPostProcessing)
            {
                cmd.Blit(null, tempColorTarget.Identifier(), settings.BlitMaterial, 2);
            }
            else
            {
                var cameraTarget = renderingData.cameraData.renderer.cameraColorTarget;
                Blit(cmd, cameraTarget, tempColorTarget.Identifier());
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempColorTarget.id);
        }
    }

    class RenderPass : ScriptableRenderPass
    {
        Settings settings;
        List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();

        FilteringSettings m_FilteringSettings;
        RenderStateBlock m_RenderStateBlock;
        RenderTargetHandle renderTarget;
        RenderTargetIdentifier depthHandle;

        public RenderTargetIdentifier RenderTarget => renderTarget.Identifier();

        public RenderPass(Settings settings)
        {
            this.settings = settings;
            renderPassEvent = settings.RenderPassEvent + settings.Offset + 1;

            m_ShaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
            m_ShaderTagIdList.Add(new ShaderTagId("UniversalForward"));
            m_ShaderTagIdList.Add(new ShaderTagId("UniversalForwardOnly"));

            m_FilteringSettings = new FilteringSettings(RenderQueueRange.all, settings.LayerMask);
            m_RenderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);

            renderTarget.Init("_GSF_RenderPass");
        }

        public void Setup(RenderTargetIdentifier depthHandle)
        {
            this.depthHandle = depthHandle;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.msaaSamples = 1;
            cmd.GetTemporaryRT(renderTarget.id, desc, FilterMode.Point);

            if ((int)settings.RenderPassEvent >= (int)RenderPassEvent.BeforeRenderingPostProcessing)
            {
                ConfigureTarget(renderTarget.Identifier(), depthHandle);
                ConfigureClear(ClearFlag.Color, Color.clear);
            }
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            DrawingSettings drawSettings = CreateDrawingSettings(m_ShaderTagIdList, ref renderingData, SortingCriteria.CommonOpaque);
            context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref m_FilteringSettings, ref m_RenderStateBlock);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(renderTarget.id);
        }
    }

    class CopyDepthPass : ScriptableRenderPass
    {
        RenderTargetHandle depthHandle;
        Settings settings;

        public RenderTargetIdentifier DepthHandle => depthHandle.Identifier();

        public CopyDepthPass(Settings settings)
        {
            this.settings = settings;

            depthHandle.Init("_DepthCopy");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.msaaSamples = 1;

            cmd.GetTemporaryRT(depthHandle.id, desc, FilterMode.Point);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get("OutlineBlitCMD");
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            cmd.Blit(depthHandle.Identifier(), depthHandle.Identifier(), settings.BlitMaterial, 1);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(depthHandle.id);
        }
    }

    class BlitPass : ScriptableRenderPass
    {
        Settings settings;
        RenderTargetIdentifier renderTarget;

        public BlitPass(Settings settings)
        {
            this.settings = settings;
            renderPassEvent = settings.RenderPassEvent + settings.Offset + 2;
        }

        public void Setup(RenderTargetIdentifier renderTarget)
        {
            this.renderTarget = renderTarget;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            var screenTex = colorAttachment;
            cmd.Blit(null, screenTex, settings.BlitMaterial, 0);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    GrabPass grabPass;
    RenderPass renderPass;
    BlitPass blitPass;
    CopyDepthPass copyDepthPass;
    [SerializeField] Settings settings = new Settings();

    public override void Create()
    {
        grabPass = new GrabPass(settings);
        renderPass = new RenderPass(settings);
        blitPass = new BlitPass(settings);
        copyDepthPass = new CopyDepthPass(settings);
        copyDepthPass.renderPassEvent = RenderPassEvent.AfterRenderingPrePasses;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if ((int)settings.RenderPassEvent >= (int)RenderPassEvent.BeforeRenderingPostProcessing)
        {
            renderer.EnqueuePass(copyDepthPass);
        }

        renderer.EnqueuePass(grabPass);

        renderPass.Setup(copyDepthPass.DepthHandle);
        renderer.EnqueuePass(renderPass);

        if ((int)settings.RenderPassEvent >= (int)RenderPassEvent.BeforeRenderingPostProcessing)
        {
            blitPass.Setup(renderPass.RenderTarget);
            renderer.EnqueuePass(blitPass);
        }
    }
}