@extends('layouts.backend.app')

@section('content')

<div class="section-body">
 <div class="row">
  <div class="col-12 col-md-12 col-lg-7">
    <div class="card">

      <div class="card-header">
        <h4>{{ __('Orders') }}</h4>
      </div>
      <div class="card-body">
        <div class="table-responsive">
          <table class="table table-hover text-center">
            <thead>
              <tr>
                
                <th scope="col">{{ __('Item Name') }}</th>
                <th scope="col">{{ __('Quantity') }}</th>
                <th scope="col">{{ __('Total Amount') }}</th>
              </tr>
            </thead>
            <tbody>
              @php
              $subtotal=0;
              @endphp
              @foreach($info->orderlist as $key => $itemrow)
              @php
              $total= $itemrow->total+$subtotal;
              $subtotal = $total; 
              @endphp
              <tr>
               
                <td>{{ $itemrow->products->title ?? '' }}</td>
                <td>{{ $itemrow->qty ?? '' }}</td>
                <td>{{ number_format($itemrow->total*$itemrow->qty,2) }}</td>
                
              </tr>
              @endforeach
            </tbody>
          </table>
        </div>
        <div class="text-left">
        
          @if($info->coupon_id != null)
          <p><b>Discount Code: </b>  {{ $info->coupon->title ?? '' }}</p>
          <p><b>Discount: </b>  {{ $info->discount }}</p>
          @endif
          <p><b>Subtotal: </b>  {{ $info->total }}</p>
          <p><b>Shipping: </b>  {{ $info->shipping }}</p>
          <p><b>Total: </b>  {{ $info->shipping+$info->total }}</p>
        </div>
      </div>
     
      <div class="card-footer">
        <form method="post" id="basicform" action="{{ route('admin.order.update',$info->id) }}">
          @csrf
         
          <div class="row">
            @if($info->order_type == 1)
            
            <div class="form-group col-lg-6">
              <label>{{ __('Select Rider') }}</label>
              <select class="form-control" name="rider">
                @foreach($riders  as $row)
                <option value="{{ $row->user_id }}">{{ $row->riders->name }} (#{{ $row->user_id }})</option>
                @endforeach
              </select>
            </div>
            
            @endif
            @if($info->order_type == 1)
            <div class="form-group col-lg-6">
             @elseif(empty($info->rider_id))
             <div class="form-group col-lg-12">
               @else
               <div class="form-group col-lg-12">
                @endif
               
                <label>{{ __('Order Status') }}</label>
                <select class="form-control" name="status">
                  <option value="2" @if($info->status==2) selected="" @endif>{{ __('Order Pending') }}</option>
                  <option value="3" @if($info->status==3) selected="" @endif>{{ __('Accept Order') }}</option>
                  <option value="1" @if($info->status==1) selected="" @endif>{{ __('Order Complete') }}</option>
                  <option value="0" @if($info->status==0) selected="" @endif>{{ __('Decline Order') }}</option>
                </select>
                
              </div>
            </div>
            <button type="submit" class="btn btn-primary col-12 submit-btn">{{ __('Processed') }}</button>
          </form>
        </div>
      </div>
      <div class="card">
        <div class="card-header">
          <h5 class="text-primary text-center">{{ __('Order Log') }}</h5>
        </div>
        <div class="card-body">

          <div class="activities">

            <div class="activity">
              <div class="activity-icon bg-primary text-white shadow-primary">
                <i class="far fa-paper-plane"></i>
              </div>
              <div class="activity-detail">
                <div class="mb-2">
                  <span class="text-job text-primary">{{ $info->created_at->diffForHumans() }}</span>
                  <span class="bullet"></span>

                </div>
                <p>{{ __('Order Created') }}</p>
              </div>
            </div>

            @foreach($info->orderlog ?? []  as $key => $row)

            <div class="activity">
              <div class="activity-icon bg-primary text-white shadow-primary">
               @if($row->status == 3)
               <i class="fas fa-comment-alt"></i>
               @elseif($row->status == 2)
               <i class="far fa-paper-plane"></i>
               @elseif($row->status == 1)
               <i class="far fa-check-square"></i>
               @elseif($row->status == 0)
               <i class="fas fa-ban"></i>
               @endif
             </div>
             <div class="activity-detail">
              <div class="mb-2">
                <span class="text-job text-primary">{{ $row->created_at->diffForHumans() }}</span>              
              </div>
              @if($row->status == 3)
              <p class="text-warning">{{ __('Order Accepted') }} </p>
              @elseif($row->status == 2)
              <p class="text-primary">{{ __('Order Created') }} </p>
              @elseif($row->status == 1)
              <p class="text-success">{{ __('Order Completed') }} </p>
              @elseif($row->status == 0)
              <p class="text-danger">{{ __('Order Cancelled') }} </p>
              @endif
            </div>
          </div>
          @endforeach
        </div>
      </div>
    </div>
    <div class="card">
      <div class="card-header">
        <h5 class="text-primary text-center">{{ __('Rider Log') }}</h5>
      </div>
      <div class="card-body">
        <div class="activities">
          @foreach($info->riderlog ?? []  as $key => $row)

          <div class="activity">
            <div class="activity-icon bg-primary text-white shadow-primary">
             
             @if($row->status == 2)
             <i class="fas fa-comment-alt"></i>
             @elseif($row->status == 1)
             <i class="far fa-check-square"></i>
             @elseif($row->status == 0)
             <i class="fas fa-ban"></i>
             @endif
           </div>
           <div class="activity-detail">
            <div class="mb-2">
              <span class="text-job text-primary">{{ $row->created_at->diffForHumans() }}</span>              
            </div>
            
            @if($row->status == 2)
            <p class="text-warning">{{ __('Order Pending') }} </p>
            @elseif($row->status == 1)
            <p class="text-success">{{ __('Order Accepted') }} </p>
            @elseif($row->status == 0)
            <p class="text-danger">{{ __('Order Cancelled') }} </p>
            @endif
          </div>
          <div class="float-right dropdown">
            <a href="#" data-toggle="dropdown" aria-expanded="false"><i class="fas fa-ellipsis-h"></i></a>
            <div class="dropdown-menu" x-placement="bottom-start" >
              <div class="dropdown-title">{{ __('Rider Info') }}</div>
              <a href="#" class="dropdown-item has-icon"><i class="far fa-id-badge"></i>ID #{{ $row->user_id }}</a>
              <a href="#" class="dropdown-item has-icon"><i class="far fa-id-badge"></i>Name: {{ $row->user->name }}</a>
              <a href="#" class="dropdown-item has-icon"><i class="fas fa-eye"></i>Order Seen @if($row->seen == 1) <span class="badge badge-success badge-sm">Yes</span> @else <span class="badge badge-danger">No</span> @endif</a>
            </div>
          </div>
        </div>
        @endforeach
      </div>
    </div>
  </div>
</div>
<div class="col-12 col-md-12 col-lg-5">
  <div class="card">

    <div class="card-body">

      <div class="profile-widget">

        <div class="profile-widget-header">                     

          <div class="profile-widget-items">
            <div class="profile-widget-item">
              <div class="profile-widget-item-label">{{ __('Amount') }}</div>
              <div class="profile-widget-item-value">{{ number_format($info->total+$info->shipping,2) }}</div>
            </div>
             <div class="profile-widget-item">
              <div class="profile-widget-item-label">{{ __('Shipping') }}</div>
              <div class="profile-widget-item-value">{{ number_format($info->shipping,2) }}</div>
            </div>
            <div class="profile-widget-item">
              <div class="profile-widget-item-label">{{ __('Payment Mode') }}</div>
              <div class="profile-widget-item-value">{{ strtoupper($info->payment_method) }}</div>
            </div>
            <div class="profile-widget-item">
              <div class="profile-widget-item-label">{{ __('Payment Status') }}</div>
              <div class="profile-widget-item-value">@if($info->payment_status == 0)
               <span class="text-danger">{{ __('Pending') }}</span>
               @elseif($info->payment_status == 1) <span class="text-success">{{ __('Completed') }}</span> @endif</div>

             </div>
             <div class="profile-widget-item">
              <div class="profile-widget-item-label">{{ __('Order Status') }}</div>
              <div class="profile-widget-item-value">@if($info->status == 0)
               <span class="text-danger">{{ __('Cancelled') }}</span>
               @elseif($info->status == 2) <span class="text-warning">{{ __('Pending') }}</span> 
               @elseif($info->status == 3) <span class="text-primary">{{ __('Accepted') }}</span> 
               @elseif($info->status == 1) <span class="text-success">{{ __('Completed') }}</span> 
               @endif
             </div>

           </div>
         </div>
       </div>
       @php
       $customerInfo=json_decode($info->data);

       @endphp
       <div class="profile-widget-description">
        <div class="profile-widget-name">{{ __('Customer Name') }}: <div class="text-muted d-inline font-weight-normal"> {{ $customerInfo->name }}</div></div>
        <div class="profile-widget-name">{{ __('Customer Phone') }}: <div class="text-muted d-inline font-weight-normal"> {{ $customerInfo->phone }}</div></div>
        <div class="font-weight-bold mb-2">{{ __('Payment Id:') }} <b> {{ $customerInfo->payment_id ?? '' }}</b></div>
       
      
        @if($info->order_type == 1)
        <div class="profile-widget-name">{{ __(' Delevery Address') }}: <div class="text-muted d-inline font-weight-normal"> {{ $customerInfo->address }}</div></div>
        @endif
        <div class="font-weight-bold mb-2">{{ __('Order Note') }}</div>
        {{ $customerInfo->note }}
      </div>
      
      @if($info->order_type == 1)
     
      <div class="card-footer text-center">
        <div class="font-weight-bold mb-2">{{ __('Order Location') }}</div>
        <div class="map_area" id="map">

        </div>
         @if(!empty($info->riderinfo))
        <div class="card mt-4">
          <div class="card-header text-center">
            <h4>{{ __('Rider Information') }}</h4>
          </div>
          <div class="card-body">
           <img alt="image" src="{{ asset($info->riderinfo->avatar) }}"  height="100"> 
            @if(!empty($info->riderinfo->info))
            @php
            $data=json_decode($info->riderinfo->info->content);
            @endphp
            <p>{{ __('Name') }} : {{ $info->riderinfo->name }}</p>
            <p>{{ __('Phone1') }} : {{ $data->phone1 }}</p>
            <p>{{ __('Phone2') }} : {{ $data->phone2 }}</p>
            <p>{{ __('Address') }} : {{ $data->full_address }}</p>
            @endif
          </div>
        </div>
        @endif
      </div>
      
      @endif

    </div>
  </div>
</div>
</div>

</div>

@endsection
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
@if($info->order_type == 1)
<script async defer src="https://maps.googleapis.com/maps/api/js?key={{ env('PLACE_KEY') }}&libraries=places&sensor=false&callback=initialise"></script>
<script type="text/javascript">
  "use strict";
 var resturent_lat = {{ $info->vendorinfo->location->latitude }};
 var resturent_long  = {{ $info->vendorinfo->location->longitude }};

 var customer_lat = {{ $customerInfo->latitude }};
 var customer_long = {{ $customerInfo->longitude }};

 var resturent_icon= '{{ asset('uploads/resturent.png') }}';
 var user_icon= '{{ asset('uploads/userpin.png') }}';

 var customer_name= '{{ $customerInfo->name }}';
 var resturent_name= '{{ $info->vendorinfo->name }}';
 var mainUrl= "{{ url('/') }}";


 function initialise(){
  var map;
  var resturent = new google.maps.LatLng(resturent_lat,resturent_long);
  var customer = new google.maps.LatLng(customer_lat,customer_long);
  var option ={
    zoom : 10,
    center : resturent, 
  };
  map = new google.maps.Map(document.getElementById('map'),option);
  var display = new google.maps.DirectionsRenderer({polylineOptions: {
    strokeColor: "rgba(255, 0, 0, 0.5)"
  }});
  var services = new google.maps.DirectionsService();
  display.setMap(map);
  function calculateroute(){
    var request ={
      origin : resturent,
      destination:customer,
      travelMode: 'DRIVING'
    };
    services.route(request,function(result,status){

      if(status =='OK'){
        display.setDirections(result);
        display.setOptions( { suppressMarkers: true } );
        var leg = result.routes[ 0 ].legs[ 0 ];
        makeMarker( leg.start_location, resturent_icon, resturent_name );
        makeMarker( leg.end_location, user_icon, customer_name );
      }
    });
  }
  function makeMarker( position, icon, title ) {
   new google.maps.Marker({
    position: position,
    map: map,
    icon: icon,
    title: title
  });
 }
 calculateroute();
}


@endif

  //response will assign this function
   function success(res){
    $('.submit-btn').remove();
  }
</script>  
@endsection
