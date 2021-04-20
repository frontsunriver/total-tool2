@extends('layouts.backend.app')

@section('content')
<section>
    <div class="section-header">
        <h4>Payment Settings</h4>  
    </div>
    <div class="section-body">
        <div class="card">
            <div class="card-header">
              <h4>Payment Settings</h4>
            </div>
            <div class="card-body">
                <form action="{{ route('admin.payment.update') }}" method="POST" id="basicform">
                @csrf
                    <ul class="nav nav-tabs payment-settings" id="myTab" role="tablist">
                        
                        <li class="nav-item active show">
                            <a class="nav-link" id="home-tab" data-toggle="tab" href="#paypal" role="tab" aria-controls="home" aria-selected="true">Paypal</a>
                        </li>
                        <li class="nav-item">
                        <a class="nav-link" id="profile-tab" data-toggle="tab" href="#instamojo" role="tab" aria-controls="profile" aria-selected="false">Instamojo</a>
                        </li>
                        <li class="nav-item">
                        <a class="nav-link" id="contact-tab" data-toggle="tab" href="#razorpay" role="tab" aria-controls="contact" aria-selected="false">Razorpay</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="contact-tab" data-toggle="tab" href="#toyyibpay" role="tab" aria-controls="contact" aria-selected="false">Toyyibpay</a>
                        </li>
                    </ul>
                    <div class="tab-content" id="myTabContent">
                       
                        <div class="tab-pane fade active show" id="paypal" role="tabpanel" aria-labelledby="profile-tab">
                            <div class="form-group mt-3">
                                <label>Paypal Client Id</label>
                                <input type="text" name="paypal_client_id" placeholder="Paypal Client Id" class="form-control" value="{{ $paymentmeta != null ? $paymentmeta->paypal_client_id : '' }}">
                            </div>
                            <div class="form-group">
                                <label>Paypal Secret</label>
                                <input type="text" name="paypal_secret" placeholder="Paypal Secret" class="form-control" value="{{ $paymentmeta != null ? $paymentmeta->paypal_secret : '' }}">
                            </div>
                            <div class="form-group">
                                <label>Paypal Status</label>
                                <select name="paypal_status" class="form-control">
                                    <option {{ $paymentmeta != null ? $paymentmeta->paypal_status == 'enabled' ? 'selected' : ''  : '' }} value="enabled">Enabled</option>
                                    <option {{ $paymentmeta != null ? $paymentmeta->paypal_status == 'disabled' ? 'selected' : ''  : '' }} value="disabled">Disabled</option>
                                </select>
                            </div>
                            <button class="btn btn-primary btn-lg">Update</button>
                        </div>
                        <div class="tab-pane fade" id="instamojo" role="tabpanel" aria-labelledby="contact-tab">
                            <div class="form-group mt-3">
                                <label>Instamojo X-Api-Key</label>
                                <input type="text" name="instamojo_x_api_key" placeholder="Instamojo X-Api-Key" class="form-control" value="{{ $paymentmeta != null ? $paymentmeta->instamojo_x_api_key : '' }}">
                            </div>
                            <div class="form-group">
                                <label>Instamojo X-Auth-Token</label>
                                <input type="text" name="instamojo_x_auto_token" placeholder="Instamojo X-Auth-Token" class="form-control" value="{{ $paymentmeta != null ? $paymentmeta->instamojo_x_auto_token : '' }}">
                            </div>
                            <div class="form-group">
                                <label>Instamojo Status</label>
                                <select name="instamojo_status" class="form-control">
                                    <option {{ $paymentmeta != null ? $paymentmeta->instamojo_status == 'enabled' ? 'selected' : ''  : '' }} value="enabled">Enabled</option>
                                    <option {{ $paymentmeta != null ? $paymentmeta->instamojo_status == 'disabled' ? 'selected' : ''  : '' }} value="disabled">Disabled</option>
                                </select>
                            </div>
                            <button class="btn btn-primary btn-lg">Update</button>
                        </div>
                        <div class="tab-pane fade" id="razorpay" role="tabpanel" aria-labelledby="contact-tab">
                            <div class="form-group mt-3">
                                <label>Razorpay Key Id</label>
                                <input type="text" name="razorpay_key_id" placeholder="Razorpay Key Id" class="form-control" value="{{ $paymentmeta != null ? $paymentmeta->razorpay_key_id : '' }}">
                            </div>
                            <div class="form-group">
                                <label>Razorpay Key Secret</label>
                                <input type="text" name="razorpay_key_secret" placeholder="Razorpay Key Secret" class="form-control" value="{{ $paymentmeta != null ? $paymentmeta->razorpay_key_secret : '' }}">
                            </div>
                            <div class="form-group">
                                <label>Razorpay Status</label>
                                <select name="razorpay_status" class="form-control">
                                    <option {{ $paymentmeta != null ? $paymentmeta->razorpay_status == 'enabled' ? 'selected' : ''  : '' }} value="enabled">Enabled</option>
                                    <option {{ $paymentmeta != null ? $paymentmeta->razorpay_status == 'disabled' ? 'selected' : ''  : '' }} value="disabled">Disabled</option>
                                </select>
                            </div>
                            <button class="btn btn-primary btn-lg">Update</button>
                        </div>
                        <div class="tab-pane fade" id="toyyibpay" role="tabpanel" aria-labelledby="contact-tab">
                            <div class="form-group mt-3">
                                <label>Toyyibpay UserSecretKey</label>
                                <input type="text" name="toyyibpay_userSecretKey" placeholder="Toyyibpay UserSecretKey" class="form-control"  value="{{ $paymentmeta != null ? $paymentmeta->toyyibpay_userSecretKey : '' }}">
                            </div>
                            <div class="form-group">
                                <label>Toyyibpay CategoryCode</label>
                                <input type="text" name="toyyibpay_categoryCode" placeholder="Toyyibpay CategoryCode" class="form-control" value="{{ $paymentmeta != null ? $paymentmeta->toyyibpay_categoryCode : '' }}">
                            </div>
                            <div class="form-group">
                                <label>Toyyibpay Status</label>
                                <select name="toyyibpay_status" class="form-control">
                                    <option {{ $paymentmeta != null ? $paymentmeta->toyyibpay_status == 'enabled' ? 'selected' : ''  : '' }} value="enabled">Enabled</option>
                                    <option {{ $paymentmeta != null ? $paymentmeta->toyyibpay_status == 'disabled' ? 'selected' : ''  : '' }} value="disabled">Disabled</option>
                                </select>
                            </div>
                            <button class="btn btn-primary btn-lg">Update</button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
</section>
@endsection

@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script>
    "use strict";
    function success(res){
		
	}    
</script> 
@endsection