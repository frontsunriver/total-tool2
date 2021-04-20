<?php 
namespace Lpress\Verify;
use Illuminate\Support\ServiceProvider;
use Illuminate\Foundation\AliasLoader;
/**
 * 
 */
class VerfiyServiceProvider extends ServiceProvider
{
	
	public function boot()
	{
		$this->loadRoutesFrom(__DIR__.'/routes/web.php');
	}

	public function register()
	{
		$loader = AliasLoader::getInstance();
		$loader->alias('Everify', 'Everify::class');
	}
}

 ?>