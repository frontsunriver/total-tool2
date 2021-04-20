<?php 


Route::group(['namespace'=>'Amcoders\Plugin\update\http\controllers','middleware'=>['web','auth','admin'],'prefix'=>'admin/', 'as'=>'admin.'],function(){

	Route::resource('update','UpdateController');

});