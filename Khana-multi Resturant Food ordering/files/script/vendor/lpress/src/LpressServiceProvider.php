<?php 
namespace Amcoders\Lpress;
use Illuminate\Support\ServiceProvider;
use Illuminate\Foundation\AliasLoader;
/**
 * 
 */
class LpressServiceProvider extends ServiceProvider
{
	
	public function boot()
	{
		$this->loadRoutesFrom(__DIR__.'/routes/web.php');
		$this->loadViewsFrom(__DIR__.'/views','lphelper');
	}

	public function register()
	{
		$loader = AliasLoader::getInstance();
		$loader->alias('Lphelper', 'Lphelper::class');
	}
}

 ?>