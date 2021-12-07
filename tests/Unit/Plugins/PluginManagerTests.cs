using System;
using System.Linq;
using Gossip.Plugins;
using Moq;
using NUnit.Framework;

namespace Gossip.UnitTests.Plugins
{
    [TestFixture]
    public class PluginManagerTests
    {

        public class When_PluginManager_has_no_plugins
        {
            [Test]
            public void It_should_produce_an_empty_enumerable()
            {
                var pluginManager = new PluginManager();
                var plugins = pluginManager.InstantiatePlugins();

                Assert.IsEmpty(plugins);
            }
        }

        public class When_PluginManager_has_plugins
        {
            [Test]
            public void It_should_instantiate_one_of_each_plugin()
            {
                var mockPlugin = new Mock<IDatabasePlugin>();
                var pluginManager = new PluginManager();
                pluginManager.AddPlugin(() => mockPlugin.Object);
                pluginManager.AddPlugin(() => mockPlugin.Object);
                pluginManager.AddPlugin(() => mockPlugin.Object);

                var plugins = pluginManager.InstantiatePlugins();

                Assert.IsTrue(plugins.Count() == 3);
            }
        }

        public class When_PluginManager_has_exception_throwing_factory
        {
            [Test]
            public void It_should_throw_a_PluginInstantiationException()
            {
                var mockPlugin = new Mock<IDatabasePlugin>();
                var pluginManager = new PluginManager();
                pluginManager.AddPlugin(() => mockPlugin.Object);
                pluginManager.AddPlugin((Func<IDatabasePlugin>)(() => throw new Exception()));
                pluginManager.AddPlugin(() => mockPlugin.Object);

                Assert.Throws<PluginInstantiationException>(() => pluginManager.InstantiatePlugins());
            }
        }
    }

    public class When_PluginManager_has_null_producing_factory
    {
        [Test]
        public void It_should_throw_a_NullPluginException()
        {
            var mockPlugin = new Mock<IDatabasePlugin>();
            var pluginManager = new PluginManager();
            pluginManager.AddPlugin(() => mockPlugin.Object);
            pluginManager.AddPlugin(() => (IDatabasePlugin)null);
            pluginManager.AddPlugin(() => mockPlugin.Object);

            Assert.Throws<NullPluginException>(() => pluginManager.InstantiatePlugins());
        }
    }
}