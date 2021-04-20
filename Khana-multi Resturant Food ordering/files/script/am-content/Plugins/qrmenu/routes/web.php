<?php 


Route::group(['prefix'=>'store','as'=>'store.','namespace'=>'Amcoders\Plugin\qrmenu\http\controllers','middleware'=>['web','auth','store']],function(){

	//here create your routes
	Route::get('qrmenu','QrmenuController@index')->name('qrmenu.index');
	Route::get('qrmenu/style','QrmenuController@style')->name('qrmenu.style');
});