@extends('layouts.backend.app')
@section('content')
<div class="row">
 <div class="col-lg-9">      
  <div class="card">
   <div class="card-body">

    <h4>{{ __('Add New Coupon') }}</h4>
    <form method="post" id="basicform" action="{{ route('store.coupon.store') }}">
     @csrf
     <div class="custom-form pt-20">

       @php
       $arr['title']= 'Coupon Code';
       $arr['id']= 'name';
       $arr['type']= 'text';
       $arr['placeholder']= 'Coupon Code';
       $arr['name']= 'title';
       $arr['is_required'] = true;

       echo  input($arr);

       $arr['title']= 'percent';
       $arr['id']= 'percent';
       $arr['type']= 'number';
       $arr['placeholder']= 'Enter percent of amount';
       $arr['name']= 'percent';
       $arr['is_required'] = true;

       echo  input($arr);

       $arr['title']= 'Expired Date';
       $arr['id']= 'expired_date';
       $arr['type']= 'date';
       $arr['placeholder']= 'Enter Expired Date';
       $arr['name']= 'expired_date';
       $arr['is_required'] = true;

       echo  input($arr);    
       @endphp

     </div>
   </div>
 </div>

</div>
<div class="col-lg-3">
  <div class="single-area">
   <div class="card">
    <div class="card-body">
     <h5>{{ __('Publish') }}</h5>
     <hr>
     <div class="btn-publish">
      <button type="submit" class="btn btn-primary col-12"><i class="fa fa-save"></i> {{ __('Save') }}</button>
    </div>
  </div>
</div>
</div>
<div class="single-area">
  <div class="card sub">
    <div class="card-body">
     <h5>{{ __('Status') }}</h5>
     <hr>
     <select class="custom-select mr-sm-2" id="inlineFormCustomSelect" name="status">
      <option value="1">{{ __('Published') }}</option>
      <option value="2">{{ __('Draft') }}</option>

    </select>
  </div>
</div>
</div>
</div>
</div>
</form>
@endsection

@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script>
  "use strict";
 //response will assign this function
 function success(res){
   location.reload();
 }

</script>
@endsection