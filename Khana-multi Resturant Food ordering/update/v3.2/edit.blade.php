@extends('layouts.backend.app')
@section('content')
<div class="row">
 <div class="col-lg-9">      
  <div class="card">
   <div class="card-body">
    
    <h4>{{ __('Edit product') }}</h4>
    <form method="post" class="basicform" action="{{ route('store.product.update',$info->id) }}">
     @csrf
     @method('PUT')
     <div class="custom-form pt-20">

       @php
       $arr['title']= 'Product Name';
       $arr['id']= 'name';
       $arr['type']= 'text';
       $arr['placeholder']= 'Product Title';
       $arr['name']= 'title';
       $arr['value']= $info->title;
       $arr['is_required'] = true;

       echo  input($arr);
       @endphp
       <div class="form-group">
        <label for="price">Price</label>
        <input type="number" placeholder="Product Price" step="any" name="price" class="form-control" id="price" required="" value="{{ $info->price->price }}" autocomplete="off" >
      </div>
      
       @php
       $arr['title']= 'Excerpt';
       $arr['id']= 'excerpt';
       $arr['placeholder']= 'short description';
       $arr['name']= 'excerpt';
       $arr['is_required'] = true;
       $arr['value']= $info->excerpt->content;

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
    <option value="1" @if($info->status == 1) selected="" @endif>{{ __('Published') }}</option>
    <option value="2" @if($info->status == 2) selected="" @endif>{{ __('Draft') }}</option>

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

      @php
      $catArr=[];

      foreach ($info->postcategory as $row) {
        array_push($catArr, $row->category_id);
      }
      
      @endphp  
       

      <?php echo AdminCategoryUpdate(1, $catArr,true); ?>

      <div class="cover-bar"></div>
    </div>
  </div>
</div>
</div>
</div>


<?php 
if(!empty($info->preview)){
  
    $media['preview'] = $info->preview->content;
    $media['value'] = $info->preview->content;
    echo  mediasection($media);
    
  
  
}
else{
 echo mediasection();
}

?>

@if(count($addons) > 0)
<div class="single-area">
 <div class="card sub">
  <div class="card-body">
   <h5>{{ __('Addon Product') }}</h5>
   <hr>
   <div class="scroll-bar-wrap">
     <div class="category-list">
       @php
      $addonArr=[];

      foreach ($info->addonid as $row) {
        array_push($addonArr, $row->addon_id);
      }
      
      @endphp

      @foreach($addons as $key => $addon)

       <div class="custom-control custom-checkbox"><input @if(in_array($addon->id, $addonArr)) checked @endif type="checkbox" name="addon[]" class="custom-control-input" value="{{ $addon->id }}" id="addon{{ $addon->id }}">
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


</div>
</div>
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
    // location.reload();
   }

</script>
@endsection