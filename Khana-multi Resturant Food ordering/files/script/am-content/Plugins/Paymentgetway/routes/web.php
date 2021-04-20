<?php 
Route::group(['namespace'=>'Amcoders\Plugin\Paymentgetway\http\controllers','middleware'=>['web','auth']],function(){
	
	Route::get('paypal-fallback','PaypalController@paypal_success_payment')->name('paypal_fallback');


	Route::get('toyyibpay-fallback','ToyyibpayController@status')->name('toyyibpay.fallback');


	Route::get('instamojo-fallback','InstamojoController@status')->name('instamojo.fallback');

	Route::post('razorpay/status','RazorpayController@status')->name('razorpay.status');

	Route::get('admin/settings/payment/info','SettingsController@index')->name('admin.payment.settings');
	Route::post('admin/payment/settings','SettingsController@update')->name('admin.payment.update');

	
});

Route::group(['namespace'=>'Amcoders\Plugin\Paymentgetway\http\controllers\subscription','middleware'=>['web','auth','store','verified','approval'],'prefix'=>'store/', 'as'=>'store.'],function(){
	
	Route::get('paypal-fallback','PaypalController@paypal_success_payment')->name('paypal_fallback');
	Route::get('toyyibpay-fallback','ToyyibpayController@status')->name('toyyibpay.fallback');
	Route::get('instamojo-fallback','InstamojoController@status')->name('instamojo.fallback');
	Route::post('razorpay/status','RazorpayController@status')->name('razorpay.status');
	Route::get('payment-with/razorpay','RazorpayController@index');
	
});