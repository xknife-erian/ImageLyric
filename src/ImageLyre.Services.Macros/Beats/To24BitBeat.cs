﻿using ImageLyre.ImageEngine;
using NLog;

namespace ImageLyre.Services.Macros.Beats;

public class To24BitBeat : BaseMacroBeat
{
    private static readonly ILogger _Log = LogManager.GetCurrentClassLogger();

    public To24BitBeat(ITarget target) : base(target)
    {
    }

    protected override bool DoSpecific()
    {
        try
        {
            ImageTarget.To24Bit();
            _Log.Info($"{ImageTarget.FileInfo.FullName} To24Bit.");
            return true;
        }
        catch (Exception e)
        {
            _Log.Warn(e);
            return false;
        }
    }

    public override void UnExecute()
    {
        throw new NotImplementedException();
    }
}