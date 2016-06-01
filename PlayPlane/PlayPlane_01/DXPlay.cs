using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.DirectSound;
using System.Threading;

namespace PlayPlane_01
{
    class DXPlay
    {
        private string musicPath;
        Form1 form;
        public DXPlay(Form1 form,string musicPath)
        {
            this.form = form;
            this.musicPath = musicPath;
        }

        public void Play()
        {
            SecondaryBuffer secBuffer;//缓冲区对象    
            Device secDev;//设备对象    
            secDev = new Device();
            secDev.SetCooperativeLevel(form, CooperativeLevel.Normal);//设置设备协作级别    
            secBuffer = new SecondaryBuffer(musicPath, secDev);//创建辅助缓冲区    
            secBuffer.Play(0, BufferPlayFlags.Default);//设置缓冲区为默认播放
        }

        delegate void DelegatePlay();
        public void ThreadPlay()
        {
            Thread t = new Thread(new ThreadStart(CorssThreadPlay));
            t.Start();
        }
        public void CorssThreadPlay()
        {
            if (form.InvokeRequired)
            {
                DelegatePlay dp = new DelegatePlay(CorssThreadPlay);
                form.Invoke(dp);
            }
            else
            {
                SecondaryBuffer secBuffer;//缓冲区对象    
                Device secDev;//设备对象    
                secDev = new Device();
                secDev.SetCooperativeLevel(form, CooperativeLevel.Normal);//设置设备协作级别    
                secBuffer = new SecondaryBuffer(musicPath, secDev);//创建辅助缓冲区    
                secBuffer.Play(0, BufferPlayFlags.Default);//设置缓冲区为默认播放
            }
        }
    }
}
