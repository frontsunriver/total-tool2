<?php 


Route::group(['namespace'=>'Amcoders\Plugin\locations\http\controllers','middleware'=>['web','auth','admin'],'prefix'=>'admin/', 'as'=>'admin.'],function(){

	
	Route::resource('location','LocationController');
    Route::post('locations/destroy','LocationController@destroy')->name('locations.destroy');

    Route::resource('badge','BadgeController');
    Route::post('badges/destroy','LocationController@destroy')->name('badges.destroy');
    Route::resource('featured','FeaturedController');

});