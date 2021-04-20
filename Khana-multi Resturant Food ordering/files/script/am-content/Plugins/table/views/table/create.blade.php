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
    <h4>{{ __('Add new Table') }}</h4>
    <form method="post" id="basicform" action="{{ route('store.table.store') }}">
     @csrf
     <div class="custom-form pt-20">

       @php
       $arr['title']= 'Table Name';
       $arr['id']= 'name';
       $arr['type']= 'text';
       $arr['placeholder']= 'Table Name';
       $arr['name']= 'title';
       $arr['is_required'] = true;

       echo  input($arr);

       $arr['title']= 'Table Total Chair';
       $arr['id']= 'chair';
       $arr['type']= 'number';
       $arr['placeholder']= 'Table Total Chair';
       $arr['name']= 'chair';
       $arr['is_required'] = true;

       echo  input($arr);

       $arr['title']= 'Table Price Per Hour';
       $arr['id']= 'price';
       $arr['type']= 'number';
       $arr['placeholder']= 'Table Price Per Hour';
       $arr['name']= 'price';
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
<script src="{{ asset('admin/js/media.js') }}"></script>
<script>
  (function ($) {
    "use strict";

    
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