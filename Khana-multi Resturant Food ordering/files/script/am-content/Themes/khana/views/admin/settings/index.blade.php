@extends('layouts.backend.app')

@section('style')
<link rel="stylesheet" href="{{ asset('admin/css/summernote.min.css') }}">
<link rel="stylesheet" href="{{ asset('admin/css/bootstrap-tagsinput.css') }}">
@endsection

@section('content')
<div class="row">
 <div class="col-lg-9">      
  <div class="card">
   <div class="card-body">
    
    <h4>{{ __('Site Settings') }}</h4>
    <form method="post" id="basicform" action="{{ route('admin.theme.setting.update') }}" enctype="multipart/form-data">
     @csrf
     <div class="custom-form pt-20">
      
     @php
     $arr['title']= 'Currency Icon';
     $arr['id']= 'currency_icon';
     $arr['type']= 'text';
     $arr['placeholder']= 'Currency Icon';
     $arr['name']= 'currency_icon';
     $arr['is_required'] = true;
     $arr['value'] = $icon->value;

     echo  input($arr); 

     $arr['title']= 'Currency Name';
     $arr['id']= 'currency_name';
     $arr['type']= 'text';
     $arr['placeholder']= 'Currency Name';
     $arr['name']= 'currency_name';
     $arr['is_required'] = true;
     $arr['value'] = $name->value;

     echo  input($arr);

     $arr['title']= 'Delivery fee per kilometre';
     $arr['id']= 'km_rate';
     $arr['type']= 'number';
     $arr['placeholder']= 'Delivery fee';
     $arr['name']= 'km_rate';
     $arr['is_required'] = true;
     $arr['value'] = $km_rate->value;

     echo  input($arr);

     $arr['title']= 'Rider Percent Of Commission Per Delevery';
     $arr['id']= 'commision';
     $arr['type']= 'number';
     $arr['placeholder']= '10%';
     $arr['name']= 'rider_commission';
     $arr['is_required'] = true;
     $arr['value'] = $com->value;    
     echo  input($arr); 

     $arr['title']= 'Contact Number';
     $arr['id']= 'number';
     $arr['type']= 'text';
     $arr['placeholder']= 'Contact Number';
     $arr['name']= 'number';
     $arr['is_required'] = true;
     $arr['value'] = $info->number;
     echo  input($arr);

     $arr['title']= 'Default latitude';
     $arr['id']= 'default_lat';
     $arr['type']= 'text';
     $arr['placeholder']= 'Default latitude';
     $arr['name']= 'default_lat';
     $arr['is_required'] = true;
     $arr['value'] = $json->default_lat ?? '';
     echo  input($arr);

     $arr['title']= 'Default longitude';
     $arr['id']= 'default_long';
     $arr['type']= 'text';
     $arr['placeholder']= 'Default longitude';
     $arr['name']= 'default_long';
     $arr['is_required'] = true;
     $arr['value'] = $json->default_long;
     echo  input($arr);

     $arr['title']= 'Default Zoom Label';
     $arr['id']= 'default_zoom';
     $arr['type']= 'text';
     $arr['placeholder']= 'Default Zoom';
     $arr['name']= 'default_zoom';
     $arr['is_required'] = true;
     $arr['value'] = $json->default_zoom;
     echo  input($arr);

     $arr['title']= 'Site Favicon';
     $arr['id']= 'favicon';
     $arr['type']= 'file';
     $arr['name']= 'favicon';
     $arr['is_required'] = false;
     echo  input($arr); 

     $arr['title']= 'lazyload Image png (40x40)';
     $arr['id']= 'lazy4';
     $arr['type']= 'file';
     $arr['name']= 'lazyload_40x40';
     $arr['is_required'] = false;
     echo  input($arr); 

     $arr['title']= 'lazyload Image png (138x135)';
     $arr['id']= 'lzy2';
     $arr['type']= 'file';
     $arr['name']= 'lazyload_138x135';
     $arr['is_required'] = false;
     echo  input($arr); 

     $arr['title']= 'lazyload Image png (250x186)';
     $arr['id']= 'lzy';
     $arr['type']= 'file';
     $arr['name']= 'lazyload_250x186';
     $arr['is_required'] = false;
     echo  input($arr); 

     
     $arr['title']= 'Store/Rider Default Image (250x186)';
     $arr['id']= 'lazyload';
     $arr['type']= 'file';
     $arr['name']= 'store';
     $arr['is_required'] = false;
     echo  input($arr); 

     $arr['title']= 'Admin Login Background Image (1575x2101)';
     $arr['id']= 'login-bg';
     $arr['type']= 'file';
     $arr['name']= 'login_bg';
     $arr['is_required'] = false;
     echo  input($arr); 
     
     $arr['title']= 'Tax (Percent)';
     $arr['id']= 'tax';
     $arr['type']= 'number';
     $arr['placeholder']= 'Enter Your Tax';
     $arr['name']= 'tax';
     $arr['is_required'] = true;
     $arr['value'] = $total_tax;
     echo  input($arr);
     @endphp
     
     <div class="form-group">
       <label for="color">Select Theme Color</label>
       <input type="color" class="form-control" name="color" value="{{ $theme_color }}">
     </div>

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