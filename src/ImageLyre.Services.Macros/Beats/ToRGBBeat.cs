﻿using ImageLyre.ImageEngine;
using NLog;

namespace ImageLyre.Services.Macros.Beats;

public class ToRGBBeat : BaseMacroBeat
{
    private static readonly ILogger _Log = LogManager.GetCurrentClassLogger();

    public ToRGBBeat(ITarget target) : base(target)
    {
    }

    protected override bool DoSpecific()
    {
        try
        {
            ImageTarget.ToRGB();
            _Log.Info($"{ImageTarget.FileInfo.FullName} ToRGB.");
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