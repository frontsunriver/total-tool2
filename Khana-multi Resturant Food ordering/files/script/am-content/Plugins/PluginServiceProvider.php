<?php 

namespace Amcoders\Plugin;

use Illuminate\Foundation\AliasLoader;
use Illuminate\Support\ServiceProvider;
/**
 * 
 */
class PluginServiceProvider extends ServiceProvider
{
	public function register()
	{
		$this->app->booting(function() {
			$loader = AliasLoader::getInstance();
			$loader->alias('Plugin', 'Plugin::class');
			$files = file_get_contents( base_path().'/am-content/Plugins/Helper.json');
			$helpers = json_decode($files, true);
			if (is_array($helpers) || is_object($helpers))
			{
				foreach ($helpers as $key => $value) {
					$loader->alias($value['name'], $value['class']);
				}
			}
			
		});
	}

	public function boot()
	{
		$file = file_get_contents( base_path().'/am-content/Plugins/plugin.json');
		$plugins = json_decode($file, true);
		foreach ($plugins as $plugin) {
			if ($plugin['status'] == 'active') {
				$this->loadRoutesFrom(__DIR__.'/'.$plugin['Text Domain'].'/routes/web.php');
				$this->loadMigrationsFrom(__DIR__.'/'.$plugin['Text Domain'].'/migrations');
				$this->loadViewsFrom(__DIR__.'/'.$plugin['Text Domain'].'/views', 'plugin');
			}
		}
	}
}