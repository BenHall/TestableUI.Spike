using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Spark;
using Spark.FileSystem;
using TestableUI.Spike;

public class SparkCompiler
{
  private readonly ISparkViewEngine _engine;

  private SparkCompiler(ISparkViewEngine engine)
  {
    _engine = engine;
  }

  private static SparkSettings GetSettings()
  {
    var settings = new SparkSettings();
    settings.AddNamespace("System");
    settings.AddNamespace("TestableUI.Spike");
    settings.AddAssembly(Assembly.GetExecutingAssembly());
    settings.AddViewFolder(ViewFolderType.FileSystem, new Dictionary<string, string> { { "basePath", @"D:\Users\Ben Hall\Documents\Visual Studio 2010\Projects\TestableUI.Spike\TestableUI.Spike\" } });
    return settings;
  }

  public static SparkCompiler Create()
  {
    SparkSettings settings = GetSettings();
    settings.SetPageBaseType(typeof(ViewDataPage));

    var engine = new SparkViewEngine(settings);

    return new SparkCompiler(engine);
  }
  
  public string Compile(string page)
  {
    var descriptor = new SparkViewDescriptor().AddTemplate(page);
    var sparkView = _engine.CreateInstance(descriptor);

    StringWriter writer = new StringWriter();
    sparkView.RenderView(writer);

    return writer.GetStringBuilder().ToString();
  }

  public string Compile<T>(string page, T viewDataModel)
  {
    var descriptor = new SparkViewDescriptor().AddTemplate(page);
    var sparkView = (ViewDataPage<T>)_engine.CreateInstance(descriptor);
    sparkView.model = viewDataModel;

    StringWriter writer = new StringWriter();
    sparkView.RenderView(writer);

    return writer.GetStringBuilder().ToString();
  }
}

public abstract class ViewDataPage : AbstractSparkView
{
  public string model { get; set; }
}

public abstract class ViewDataPage<T> : AbstractSparkView
{
  public T model { get; set; }
}