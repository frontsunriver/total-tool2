@extends('layouts.backend.app')
@section('content')
<div class="row">
 <div class="col-lg-9">      
  <div class="card">
   <div class="card-body">
     <div class="alert alert-danger none errorarea">
      <ul id="errors">

      </ul>
    </div>
    <h4>{{ __('Add new product') }}</h4>
    <form method="post" class="basicform" action="{{ route('store.product.store') }}">
     @csrf
     <div class="custom-form pt-20">

       @php
       $arr['title']= 'Product Name';
       $arr['id']= 'name';
       $arr['type']= 'text';
       $arr['placeholder']= 'Product Title';
       $arr['name']= 'title';
       $arr['is_required'] = true;

       echo  input($arr);
       @endphp

       <div class="form-group">
        <label for="price">Price</label>
        <input type="text" placeholder="Product Price" name="price" class="form-control" id="price" required="" value="" autocomplete="off">
      </div>

      

       @php
       $arr['title']= 'Excerpt';
       $arr['id']= 'excerpt';
       $arr['placeholder']= 'short description';
       $arr['name']= 'excerpt';
       $arr['is_required'] = true;

       echo  textarea($arr);


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
      <button type="submit" class="btn btn-primary col-12 basicbtn"><i class="fa fa-save"></i> {{ __('Save') }}</button>
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

<div class="single-area">
 <div class="card sub">
  <div class="card-body">
   <h5>{{ __('Categories') }}</h5>
   <hr>
   <div class="scroll-bar-wrap">
     <div class="category-list">
       {{ AdminCategory(1) }}
       <div class="cover-bar"></div>
     </div>
   </div>
 </div>
</div>
</div>


{{ mediasection() }}

@if(count($addons) > 0)
<div class="single-area">
 <div class="card sub">
  <div class="card-body">
   <h5>{{ __('Addon Product') }}</h5>
   <hr>
   <div class="scroll-bar-wrap">
     <div class="category-list">
      @foreach($addons as $key => $addon)
       <div class="custom-control custom-checkbox"><input type="checkbox" name="addon[]" class="custom-control-input" value="{{ $addon->id }}" id="addon{{ $addon->id }}">
        <label class="custom-control-label" for="addon{{ $addon->id }}">{{ $addon->title }}
        </label>
      </div>
      @endforeach
    </div>
  </div>
</div>
</div>
</div>
@endif

</form>

{{ mediasingle() }}
@endsection

@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script src="{{ asset('admin/js/media.js') }}"></script>
<script>
   "use strict";
  (function ($) {
    $('.use').on('click',function(){

      $('#preview').attr('src',myradiovalue);
      $('#preview_input').val(myradiovalue);
      
    });
  })(jQuery);
   //response will assign this function
   function success(res){
     location.reload();
   }

</script>
@endsection