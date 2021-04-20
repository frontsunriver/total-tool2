@extends('layouts.backend.app')
@section('content')
<div class="row">
 <div class="col-lg-9">      
  <div class="card">
   <div class="card-body">
    
    <h4>{{ __('My Information') }}</h4>
    <form method="post" id="basicform" action="{{ route('rider.my.information.update') }}">
     @csrf
     <div class="pt-20">

       @php
       $json=json_decode($info->info->content ?? '');
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

      @endphp

       <div class="form-group">
         <label>{{ __('Select Your City') }}</label>
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


</form>


@endsection

@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>


<script async defer src="https://maps.googleapis.com/maps/api/js?key={{ env('PLACE_KEY') }}&libraries=places&callback=initialize"></script>
<script src="{{ theme_asset('khana/public/js/information.js') }}"></script>
@endsection