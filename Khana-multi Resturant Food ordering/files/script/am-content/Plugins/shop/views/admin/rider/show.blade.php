@extends('layouts.backend.app')

@section('content')

@include('layouts.backend.partials.headersection',['title'=> $info->name.' Information'])

<div class="row">
  <div class="col-sm-4">
    <div class="card">
      <div class="card-header">
        <h5>{{ __('Primary Information') }}</h5>
      </div>
      <form method="post" action="{{ route('admin.vendor.update',$info->id) }}" id="basicform">
        @csrf
        <div class="card-body">
          <div class="form-group">
            <label>{{ __('Name') }}</label>
            <input type="text" name="name" class="form-control" autocomplete="off" value="{{ $info->name }}" required="">
          </div>
          <div class="form-group">
            <label>{{ __('Email') }}</label>
            <input type="email" name="email" class="form-control" autocomplete="off" value="{{ $info->email }}" required>
          </div>
          <div class="form-group">
            <label>{{ __('Password') }}</label>
            <input type="password" name="password" class="form-control" autocomplete="off" minlength="8">
            
          </div>
          <div class="form-group">
            <label>{{ __('Status') }}</label>
            <select class="form-control" name="status">
             <option value="pending" @if($info->status=='pending') selected @endif>{{ __('Pending') }}</option>
             <option value="approved" @if($info->status=='approved') selected @endif>{{ __('Approved') }}</option>
             <option value="suspended" @if($info->status=='suspended') selected @endif>{{ __('Suspended') }}</option>
             <option value="cancelled" @if($info->status=='cancelled') selected @endif>{{ __('Cancelled') }}</option>
           </select>
         </div>
         <div class="form-group">

           <input type="checkbox" name="password_change" value="1" id="check">
           <label for="check"><span class="text-danger">{{ __('With Change Password') }}</span></label>
         </div>
         <button class="btn btn-primary col-12" type="submit">{{ __('Update') }}</button>
       </div>
       
     </form>
   </div>
 </div>
 <div class="col-sm-8">
  <div class="card">
   <div class="card-header">
    <h5>{{ __('Other Information') }}</h5>
  </div>

  <div class="card-body">
   <div class="row">
     @if(!empty($info->info->content))
     <div class="col-sm-5">

      @php
     
      $json=json_decode($info->info->content);
      @endphp
      
       <p><b>{{ __('Contact Phone 1:') }} </b>{{ $json->phone1 ?? '' }}</p>
       <p><b>{{ __('Contact Phone 2:') }} </b>{{ $json->phone2 ?? '' }}</p>
       <p><b>{{ __('City:') }} </b>{{ $info->resturentlocationwithcity->area->title ?? '' }}</p>
    
       <p><b>{{ __('Location line:') }} </b>{{ $json->full_address ?? '' }}</p>
      
     </div>
     @endif
     <div class="col-sm-7 map-canvas-register"  id="map-canvas" ></div>
   </div>
 </div>
</div>
</div>
<div class="col-sm-8">
  <div class="card">
    <div class="card-header">
      <h6>{{ __('Order History') }}</h6>
    </div>
    <div class="card-body">
      <div class="table-responsive">
        <table class="table table-striped table-hover text-center table-borderless">
          <thead>
            <tr>
              <th>{{ __('Order ID') }}</th>
              <th>{{ __('Status') }}</th>
              <th>{{ __('Amount') }}</th>
             
              <th>{{ __('Created at') }}</th>
              <th>{{ __('View') }}</th>
            </tr>
          </thead>
          <tbody>
            @foreach($orders as $row)

            <tr>
              <td><a href="{{ route('admin.order.details',$row->id) }}">#{{ $row->id }}</a></td>
               <td>
              @if($row->status == 1) 
                <span class="badge badge-success">{{ __('Completed') }}</span>
                 @elseif($row->status == 2) 
                 <span class="badge badge-primary"> {{ __('Pending') }} </span>
                 @elseif($row->status == 3) <span class="badge badge-warning"> {{ __('Accepted') }} </span> 
                 @elseif($row->status == 0)  <span class="badge badge-danger"> {{ __('Cancelled') }} </span> 
               @endif
             </td>
             
              <td>{{ $row->total+$row->shipping }}</td>
                           
              <td>{{ $row->created_at->diffforHumans() }}</td>
              <td><a href="{{ route('admin.order.details',$row->id) }}" class="btn btn-primary"><i class="fas fa-eye"></i></a></td>
            </tr>
            @endforeach
          </tbody>
          <tfoot>
            <tr>
              <th>{{ __('Order ID') }}</th>
              <th>{{ __('Status') }}</th>
              <th>{{ __('Amount') }}</th>
              
              <th>{{ __('Created at') }}</th>
              <th>{{ __('View') }}</th>
            </tr>
          </tfoot>
        </table>
        {{ $orders->links() }}

      </div>
    </div>
  </div>
</div>
<div class="col-sm-4">
  <div class="card">
    <div class="card-header">
      <h6>{{ __('Overview Of Earnings') }}</h6>
    </div>
    <div class="card-body">
      <ul class="list-group">
        <li class="list-group-item d-flex justify-content-between align-items-center">
          {{ __('Total Order Completed') }}
          <span class="badge badge-primary badge-pill">{{ $total_orders }}</span>
          
        </li>
        <li class="list-group-item d-flex justify-content-between align-items-center">
          {{ __('Total Earnings') }}
          <span class="badge badge-primary badge-pill">{{ $total_earning }}</span>
        </li>
        <li class="list-group-item d-flex justify-content-between align-items-center">
          {{ __('Current Balance') }}
          <span class="badge badge-primary badge-pill">{{ $total_earning-$total_withdraw }}</span>
        </li>
        <li class="list-group-item d-flex justify-content-between align-items-center">
          {{ __('Total Withdraws') }}
          <span class="badge badge-primary badge-pill">{{ $total_withdraw }}</span>
        </li>
      </ul>
    </div>
  </div>
</div>
<div class="col-6 mt-2">
  <div class="card">
    <div class="card-header">
      <h6>{{ __('Onesignal History') }}</h6>
    </div>
    <div class="card-body">
      <div class="table-responsive">
        <table class="table table-striped table-hover text-center table-borderless">
          <thead>
            <tr>
              <th>{{ __('Signal ID') }}</th>
              <th>{{ __('Register At') }}</th>
              <th>{{ __('Delete') }}</th>

            </tr>
          </thead>
          <tbody>
            @foreach($info->Onesignal ?? [] as $row)
            <tr>
              <td>{{ $row->player_id }}</td>
              <td>{{ $row->created_at->diffforHumans() }}</td>
              <td><a href="{{ route('admin.signal.remove',$row->id) }}" class="btn btn-danger btn-danger"><i class="far fa-trash-alt"></i></a></td>
            </tr>
            @endforeach
          </tbody>
          <tfoot>
            <tr>
              <th>{{ __('Signal ID') }}</th>
              <th>{{ __('Register At') }}</th>
              <th>{{ __('Delete') }}</th>
            </tr>
          </tfoot>
        </table>

      </div>
    </div>
  </div>
</div>

<div class="col-6 mt-2">
  <div class="card">
    <div class="card-header">
      <h6>{{ __('Transaction History') }}</h6>
    </div>
    <div class="card-body">
      <div class="table-responsive">
        <table class="table table-striped table-hover text-center table-borderless">
          <thead>
            <tr>
              <th>{{ __('Transection ID') }}</th>
              <th>{{ __('Amount') }}</th>
              <th>{{ __('Status') }}</th>
              <th>{{ __('View') }}</th>
            </tr>
          </thead>
          <tbody>
            @foreach($transactions as $row)
            <tr>
              <td><a href="{{ route('admin.payout.show',$row->id) }}">#{{ $row->id }}</a></td>
              <td>{{ $row->amount }}</td>
              <td>
              @if($row->status==0)
                <span class="badge badge-danger">{{ __('Canceled') }}</span>
                @elseif($row->status==1)
                <span class="badge badge-success">{{ __('Completed') }}</span> @elseif($row->status==2)
                <span class="badge badge-primary">
                  {{ __('Processing') }}

                @endif
              </td>
              <td><a href="{{ route('admin.payout.show',$row->id) }}" class="btn btn-primary btn-sm"><i class="fas fa-eye"></i></a></td>
            </tr>
            @endforeach
          </tbody>
          <tfoot>
            <tr>
              <th>{{ __('Transection ID') }}</th>
              <th>{{ __('Amount') }}</th>
              <th>{{ __('Status') }}</th>
              <th>{{ __('View') }}</th>
            </tr>
          </tfoot>
        </table>
        {{ $transactions->links() }}

      </div>
    </div>
  </div>
</div>
</div>

<input type="hidden" value="{{ $info->resturentlocationwithcity->latitude ?? 00.00 }}" id="latitude">
<input type="hidden" value="{{ $info->resturentlocationwithcity->longitude ?? 00.00 }}" id="longitude">
@endsection

@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script async defer src="https://maps.googleapis.com/maps/api/js?key={{ env('PLACE_KEY') }}&libraries=places&callback=initialize"></script>
<script>
  "use strict";
function initialize() {

  var mapOptions, map, marker, searchBox, city,
    infoWindow = '',
   
    latEl = document.querySelector( '#latitude' ),
    longEl = document.querySelector( '#longitude' ),
    element = document.getElementById( 'map-canvas' );
    city = document.querySelector( '#city' );

  mapOptions = {
    // How far the maps zooms in.
    zoom: 13,
    // Current Lat and Long position of the pin/
    center: new google.maps.LatLng( $('#latitude').val(), $('#longitude').val()),
   
    disableDefaultUI: false, // Disables the controls like zoom control on the map if set to true
    scrollWheel: true, // If set to false disables the scrolling on the map.
    draggable: true, // If set to false , you cannot move the map around.
    // mapTypeId: google.maps.MapTypeId.HYBRID, // If set to HYBRID its between sat and ROADMAP, Can be set to SATELLITE as well.
     maxZoom: 21, // Wont allow you to zoom more than this
  };

  /**
   * Creates the map using google function google.maps.Map() by passing the id of canvas and
   * mapOptions object that we just created above as its parameters.
   *
   */
  // Create an object map with the constructor function Map()
  map = new google.maps.Map( element, mapOptions ); // Till this like of code it loads up the map.

  /**
   * Creates the marker on the map
   *
   */
  marker = new google.maps.Marker({
    position: mapOptions.center,
    map: map,
    draggable: false
  });


}
</script>

@endsection