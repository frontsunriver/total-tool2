<?php 
use Amcoders\Lpress\Lphelper;

//eval(Lphelper::Token(''));
Route::group(['namespace'=>'Laravel\Larafy\Http\Controllers','middleware' => ['web']], function(){
	Route::get('install','LarafyController@install')->name('install');
	Route::get('install/purchase','LarafyController@purchase')->name('purchase');
	Route::post('install/purchase_check','LarafyController@purchase_check')->name('purchase_check');
	Route::get('install/check','LarafyController@check')->name('install.check');
	Route::get('install/info','LarafyController@info')->name('install.info');
	Route::get('install/migrate','LarafyController@migrate')->name('install.migrate');
	Route::get('install/seed','LarafyController@seed')->name('install.seed');
	Route::post('install/store','LarafyController@send');

});


 ?>