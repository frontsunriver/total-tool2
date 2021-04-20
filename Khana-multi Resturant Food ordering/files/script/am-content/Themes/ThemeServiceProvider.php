<?php 

namespace Amcoders\Theme;
use Illuminate\Support\ServiceProvider;
use Illuminate\Foundation\AliasLoader;
/**
 * 
 */
class ThemeServiceProvider extends ServiceProvider
{
	public function register()
	{
		$this->app->booting(function() {
			$loader = AliasLoader::getInstance();
			
			$files = file_get_contents( base_path().'/am-content/Themes/Helper.json');
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
		$file = file_get_contents( base_path().'/am-content/Themes/theme.json');
		$themes = json_decode($file, true);
		foreach ($themes as $theme) {
			if ($theme['status'] == 'active') {
				$this->loadRoutesFrom(__DIR__.'/'.$theme['Text Domain'].'/routes/web.php');
				$this->loadMigrationsFrom(__DIR__.'/'.$theme['Text Domain'].'/migrations');
				$this->loadViewsFrom(__DIR__.'/'.$theme['Text Domain'].'/views', 'theme');
			}
		}
	}
}