<?php 
namespace Laravel\Larafy;
use Illuminate\Support\ServiceProvider;
use Illuminate\Foundation\AliasLoader;
class LarafyServiceProvider extends ServiceProvider
{
	
	public function boot()
	{
		$this->loadRoutesFrom(__DIR__.'/routes/web.php');
		$this->loadViewsFrom(__DIR__.'/views','Larafy');
	}

	public function register()
	{
		
	}
}

 ?>