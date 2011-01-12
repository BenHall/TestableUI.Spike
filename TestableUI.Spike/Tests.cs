using System.IO;
using System.Net;
using FootReST;
using NUnit.Framework;

namespace TestableUI.Spike.Tests
{
  [TestFixture]
  public class SparkCompilerTests
  {
    [Test]
    public void Can_compile_spark_text()
    {
      SparkCompiler compiler = SparkCompiler.Create();
      string compiledHtml = compiler.Compile(@"CanWeCompile.spark");
      Assert.IsTrue(compiledHtml.Contains("We have compiled!!"));
      Assert.IsFalse(compiledHtml.Contains("Something has gone wrong!!"));
    }

    [Test]
    public void Can_pull_data_from_model()
    {
      SparkCompiler compiler = SparkCompiler.Create();
      string compiledHtml = compiler.Compile(@"WithModel.spark", new ViewDataModel{Data="Special String"});
      Assert.IsTrue(compiledHtml.Contains("Special String"));
    }
  }

  [TestFixture]
  public class ServingPagesTests
  {
    Server server;

    [SetUp]
    public void Setup()
    {
      SparkCompiler compiler = SparkCompiler.Create();
      string compiledHtml = compiler.Compile(@"CanWeCompile.spark");

      StartServer(compiledHtml);
    }

    private void StartServer(string compiledHtml)
    {
      server = new Server();
      server.Start();
      server.DefineCustomResponse("GET", "/", compiledHtml);
    }

    [TearDown]
    public void Teardown()
    {
      server.Close();
    }

    [Test]
    public void Can_we_serve_html_compiled_text()
    {
      WebRequest request = WebRequest.Create("http://localhost:5984");
      string s = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
      Assert.IsTrue(s.Contains("We have compiled!!"));
    }
  }

}