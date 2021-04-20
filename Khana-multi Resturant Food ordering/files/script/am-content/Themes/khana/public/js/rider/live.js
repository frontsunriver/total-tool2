"use strict";

  var newOrders = [];
  getData();

  setInterval(function(){ getData(); }, 5000);

  function getData(){
    $.ajax({
      type: 'get',
      url: mainUrl+'/rider/orders/latest',
      dataType: 'json',
      contentType: false,
      cache: false,
      processData:false,

      success: function(response){ 
        
        newOrders = response.newOrders;
        RenderNewOrders();
      },
      
    })
  }

  function RenderNewOrders() {
    $('.neworder').remove();
    $.each(newOrders, function(index, value){
      if (value.seen == 0) {
        NotifyNewOrder(value.id);
      }
      var html='<div class="card card-primary neworder"><div class="card-body"><div class="row align-items-center"><div class="col"><i class="far fa-bell text-primary"></i><small>Ordered At : '+value.time+'</small><h5 class="mb-0"><a href="'+mainUrl+'/rider/order/'+value.id+'" class="text-primary">Order No #'+value.id+'</a></h5><small>'+currency+" "+value.total+'</small><br /></div><div class="col-auto"><a href="'+mainUrl+'/rider/order/'+value.id+'" class="btn btn-sm btn-primary"><i class="far fa-eye"></i></a></div></div></div></div>';
      $('#neworders').append(html);
    });
  }


  function NotifyNewOrder(id) {
    var ring= ringurl;
    
    SweetAudio('warning','You Got A New Order',10000,ring);
     $.ajax({
      type: 'get',
      url: mainUrl+'/rider/orders/latest',
      data: {id:id},
           
    })
  }
