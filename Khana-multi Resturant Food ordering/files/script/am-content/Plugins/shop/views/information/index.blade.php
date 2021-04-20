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

    <h4>{{ __('My Information') }}</h4>
    <form method="post" id="basicform" action="{{ route('store.my.information.update') }}">
     @csrf
     <div class="pt-20">

       @php
       $arr['title']= 'Delivery Time';
       $arr['id']= 'delivery';
       $arr['type']= 'number';
       $arr['placeholder']= 'Enter Maximum Delivery Time';
       $arr['name']= 'delivery';
       $arr['is_required'] = true;
       $arr['value'] = $info->delivery->content ?? '';

       echo  input($arr);

       $arr['title']= 'Pick Up Time';
       $arr['id']= 'pickup';
       $arr['type']= 'number';
       $arr['placeholder']= 'Enter Maximum Pick Up Time';
       $arr['name']= 'pickup';
       $arr['is_required'] = true;
       $arr['value'] = $info->pickup->content ?? '';


       echo  input($arr);
       $json=json_decode($info->info->content ?? '');

       $arr['title']= 'Shop Description';
       $arr['id']= 'description';
       $arr['placeholder']= 'Shop description';
       $arr['name']= 'description';
       $arr['maxlength']= 250;
       $arr['is_required'] = true;
       $arr['value'] = $json->description ?? '';

       echo  textarea($arr);
       
       // $arr['title']= 'tawk.to property ID (for Livechat)';
       // $arr['id']= 'property_id';
       // $arr['type']= 'text';
       // $arr['placeholder']= 'tawk.to property ID';
       // $arr['name']= 'property_id';
       // $arr['is_required'] = false;
       // $arr['value'] = $info->livechat->content ?? '';
      

       // echo  input($arr);

       $arr['title']= 'Support Phone Number 1';
       $arr['id']= 'phone1';
       $arr['type']= 'number';
       $arr['placeholder']= 'Support Phone Number';
       $arr['name']= 'phone1';
       $arr['is_required'] = true;
       $arr['value'] = $json->phone1 ?? '';

       echo  input($arr);

       $arr['title']= 'Support Phone Number 2';
       $arr['id']= 'phone2';
       $arr['type']= 'number';
       $arr['placeholder']= 'Support Phone Number';
       $arr['name']= 'phone2';
       $arr['value'] = $json->phone2 ?? '';
       

       echo  input($arr);

       $arr['title']= 'Support Email 1';
       $arr['id']= 'email1';
       $arr['type']= 'email';
       $arr['placeholder']= 'Support Email';
       $arr['name']= 'email1';
       $arr['value'] = $json->email1 ?? '';
       
       echo  input($arr);

       $arr['title']= 'Support Email 2';
       $arr['id']= 'email2';
       $arr['type']= 'email';
       $arr['placeholder']= 'Support Email';
       $arr['name']= 'email2';
       $arr['value'] = $json->email2 ?? '';
       
       echo  input($arr);


       $arr['title']= 'Address Line';
       $arr['id']= 'address_line';
       $arr['type']= 'text';
       $arr['placeholder']= 'Address Line';
       $arr['name']= 'address_line';
       $arr['is_required'] = true;
       $arr['value'] = $json->address_line ?? '';

       echo  input($arr);

      @endphp

       <div class="form-group">
         <label >{{ __('Select Your City') }}</label>
         <select class="form-control" name="city" >
           @php
           $locations=\App\Terms::where('type',2)->where('status',1)->get();
          
         
           $loc_id=$info->location->term_id ?? 0;
           @endphp
           
           @foreach($locations as $key => $row)
           <option value="{{ $row->id }}" @if($loc_id == $row->id) selected @endif>{{ $row->title }}</option>
           @endforeach
         </select>
       </div>
       @php
       $arr['title']= 'Full address';
       $arr['id']= 'location_input';
       $arr['type']= 'text';
       $arr['placeholder']= 'Enter Full Address';
       $arr['name']= 'full_address';
       $arr['is_required'] = true;
       $arr['value'] = $json->full_address ?? '';

       echo  input($arr);

       @endphp
       <label>Drag Your Address</label>
       <div id="map-canvas" class="map-canvas"></div>

       <input type="hidden" name="latitude" id="latitude" value="{{ $info->location->latitude ?? '00.00' }}">
       <input type="hidden" name="longitude" id="longitude" value="{{ $info->location->longitude ?? '00.00' }}">
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



<?php 
if(!empty($info->preview)){
  
    $media['title'] = 'Shop Banner Image';
    $media['preview'] = $info->preview->content;
    $media['value'] = $info->preview->content;
    echo  mediasection($media);
    
}
else{
   echo mediasection(array('title'=>'Shop Banner Image'));
}

?>

@php
$usercat=[];
foreach ($info->usercategory ?? [] as $key => $row) {
 array_push($usercat, $row->category_id);
}
@endphp



<div class="single-area">
 <div class="card sub">
  <div class="card-body">
   <h5>{{ __('Shop Tags') }}</h5>
   <hr>
   <div class="scroll-bar-wrap">
     <div class="category-list">
      <?php echo AdminCategoryUpdate(2, $usercat); ?>
       
       <div class="cover-bar"></div>
     </div>
   </div>
 </div>
</div>
</div>


<?php 
if(!empty($info->gallery)){
    $gllarr=explode(',', $info->gallery->content);

    $media['title'] = 'Shop Gallery Image';
    $media['preview_id'] = 'gallery';
    $media['input_id'] = 'gallary_input';
    $media['input_name'] = 'gallary_input';
    $media['class'] = 'multi-img';
    $media['preview'] = $gllarr;
    $media['value'] = $info->gallery->content;
    echo  mediasectionmulti($media);
    
}
else{
   echo mediasectionmulti(array('title'=>'Shop Gallery Image','preview_id'=>'gallery','input_id'=>'gallary_input','class'=>'multi-img','input_name'=>'gallary_input'));
}

?>
</form>

{{ mediasingle() }}
{{ mediamulti() }}
@endsection

@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script src="{{ asset('admin/js/media.js') }}"></script>

<script async defer src="https://maps.googleapis.com/maps/api/js?key={{ env('PLACE_KEY') }}&libraries=places&callback=initialize"></script>
<script src="{{ theme_asset('khana/public/js/information.js') }}"></script>
<script>
   "use strict";
  (function ($) {
      $('.use').on('click',function(){

      $('#preview').attr('src',myradiovalue);
      $('#preview_input').val(myradiovalue);
      
    });

   $('.use1').on('click',function(){
      $('.multi-img').hide();
      $('.gallary-src').remove();
      $('#gallery').remove()

      $.each(mycheckboxvalue, function(index, value){
        $("#gallary-img").append('<img class="gallary-src" height="80" src="' + value + '" />');
      });
      $('#gallery').remove()
      $('#gallary_input').val(mycheckboxvalue.toString())

    });
  })(jQuery);

</script>
@endsection