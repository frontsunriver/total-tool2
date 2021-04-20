<?php 


Route::group(['as' =>'admin.shop.','prefix'=>'admin/shop','namespace'=>'Amcoders\Plugin\product\http\controllers','middleware'=>'web','auth','admin'],function(){

	Route::resource('category','CategoryController');
	Route::post('categories/destroy','CategoryController@destroy')->name('productcategory');


});

Route::group(['as' =>'admin.','prefix'=>'admin/','namespace'=>'Amcoders\Plugin\product\http\controllers','middleware'=>'web','auth','admin'],function(){

	Route::get('/product/all','ProductController@index')->name('product.index');
	Route::post('/products/destroy','ProductController@destroy')->name('product.destroy');
	Route::get('earnings','EarningController@index')->name('earning.index');
	Route::get('earnings/date','EarningController@date')->name('earning.date');

	Route::get('earnings/delivery','EarningController@delivery')->name('earning.delivery');
	Route::get('earnings/saas','EarningController@saas')->name('earning.saas');

});