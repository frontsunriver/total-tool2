@extends('layouts.backend.app')
@section('content')
<div class="row">
  <div class="col-sm-4">
    <div class="card">
      <div class="card-title text-center">
        <h5 class="mt-2">{{ __('New Orders') }}</h5>
      </div>
      <div class="card-body" id="newOrders">
      </div>
    </div>
  </div>

  <div class="col-sm-4">
    <div class="card">
      <div class="card-title text-center">
        <h5 class="mt-2">{{ __('Accepted Orders') }}</h5>
      </div>
      <div class="card-body" id="OrdersAccept">
       
      </div>
    </div>
  </div>

  <div class="col-sm-4">
    <div class="card">
      <div class="card-title text-center">
        <h5 class="mt-2">{{ __('Orders Completed') }}</h5>
      </div>
      <div class="card-body" id="OrdersCompleted">
        
      </div>
    </div>
  </div>
</div>

<input type="hidden" value="{{ url('uploads/audio/ring.mp3') }}" id="ringurl">
@php
$currency=\App\Options::where('key','currency_name')->select('value')->first();
@endphp
@endsection

@section('script')

<script src="{{ asset('admin/js/form.js') }}"></script>
<script>
  var baseurl= "{{ route('store.order.create') }}";
  var mainUrl= "{{ url('/') }}";
  var currency= "{{ strtoupper($currency->value) }}";
</script>
<script>
  "use strict";
  var CompleteOrders = [];
  var OrderAccepts = [];
  var newOrders = [];
  getData();

  setInterval(function(){ getData(); }, 5000);

  function getData(){
    $.ajax({
      type: 'get',
      url: baseurl,
      dataType: 'json',
      contentType: false,
      cache: false,
      processData:false,

      success: function(response){ 
        CompleteOrders = response.CompleteOrders;
        OrderAccepts = response.OrderAccepts;
        newOrders = response.newOrders;

        RenderNewOrders();
        RenderOrdersAccept();
        RenderOrdersCompleted();
      },
      
    })
  }

  function RenderNewOrders() {
    $('.neworder').remove();
    $.each(newOrders, function(index, value){
      if (value.seen == 0) {
        NotifyNewOrder(value.id);
      }
      var html='<div class="card card-primary neworder"><div class="card-body"><div class="row align-items-center"><div class="col"><i class="far fa-bell text-primary"></i><small>Ordered At : '+value.time+'</small><h5 class="mb-0"><a href="'+mainUrl+'/store/order/'+value.id+'" class="text-primary">Order No #'+value.id+'</a></h5><small>'+currency+" "+value.total+'</small><br /></div><div class="col-auto"><a href="'+mainUrl+'/store/order/'+value.id+'" class="btn btn-sm btn-primary"><i class="far fa-eye"></i></a></div></div></div></div>';
      $('#newOrders').append(html);
    });
  }

  function RenderOrdersAccept() {
    
    $.each(OrderAccepts, function(index, value){

      var html='<div class="card card-warning neworder"><div class="card-body"><div class="row align-items-center"><div class="col"><i class="fas fa-user-clock text-warning"></i><small> Accepted At : '+value.time+'</small><h5 class="mb-0"><a href="'+mainUrl+'/store/order/'+value.id+'" class="text-warning">Order No #'+value.id+'</a></h5><small>'+currency+" "+value.total+'</small><br /></div><div class="col-auto"><a href="'+mainUrl+'/store/order/'+value.id+'" class="btn btn-sm btn-warning"><i class="far fa-eye"></i></a></div></div></div></div>';
      $('#OrdersAccept').append(html);
    });
  }

  function RenderOrdersCompleted() {
    $.each(CompleteOrders, function(index, value){

      var html='<div class="card card-success neworder"><div class="card-body"><div class="row align-items-center"><div class="col"><i class="far fa-check-circle text-success"></i><small> Ordered At : '+value.time+'</small><h5 class="mb-0"><a href="'+mainUrl+'/store/order/'+value.id+'" class="text-success">Order No #'+value.id+'</a></h5><small>'+currency+" "+value.total+'</small><br /></div><div class="col-auto"><a href="'+mainUrl+'/store/order/'+value.id+'" class="btn btn-sm btn-success"><i class="far fa-eye"></i></a></div></div></div></div>';
      $('#OrdersCompleted').append(html);
    });
  }

  function NotifyNewOrder(id) {
    var ring= $('#ringurl').val();
    
    SweetAudio('warning','You Got A New Order',10000,ring);
     $.ajax({
      type: 'get',
      url: baseurl,
      data: {id:id},
           
    })
  }
 //response will assign this function
 function success(res){
   location.reload();
 }

</script>
@endsection