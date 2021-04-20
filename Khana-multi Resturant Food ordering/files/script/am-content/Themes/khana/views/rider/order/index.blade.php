@extends('layouts.backend.app')
@section('content')
@include('layouts.backend.partials.headersection',['title'=>'My Orders'])
@php
$currency=\App\Options::where('key','currency_name')->select('value')->first();
@endphp
<div class="row">

  <div class="col-12">
    <div class="card mb-0">
      <div class="card-body">
        <ul class="nav nav-pills">
          <li class="nav-item">
            <a class="nav-link @if(url()->full() ==  route('rider.orders')) active @endif" href="{{ route('rider.orders') }}">{{ __('All Orders') }}<span class="badge @if(url()->full() ==  route('rider.orders')) badge-white @else badge-info @endif">{{ $allorders ?? \App\Riderlog::where('user_id',Auth::id())->count() }}</span></a>
          </li>
          <li class="nav-item">
            <a class="nav-link @if(url()->full() ==  route('rider.orders','status=1')) active @endif" href="{{ route('rider.orders','status=1') }}">{{ __('Accepted Orders') }}<span class="badge badge-primary">{{ $accepted ??  \App\Riderlog::where('user_id',Auth::id())->where('status',1)->count() }}</span></a>
          </li>
          <li class="nav-item">
            <a class="nav-link @if(url()->full() ==  route('rider.orders','status=2')) active @endif" href="{{ route('rider.orders','status=2') }}">{{ __('Pending Orders') }}<span class="badge badge-warning">{{ $pendings ?? \App\Riderlog::where('user_id',Auth::id())->where('status',2)->count() }}</span></a>
          </li>
          <li class="nav-item">
            <a class="nav-link @if(url()->full() ==  route('rider.orders','status=0')) active @endif" href="{{ route('rider.orders','status=0') }}">{{ __('Declined Orders') }}<span class="badge badge-danger">{{ $declineds ?? \App\Riderlog::where('user_id',Auth::id())->where('status',0)->count() }}</span></a>
          </li>
          <li class="nav-item ">
            <a class="nav-link @if(url()->full() ==  route('rider.orders','status=complete')) active @endif" href="{{ route('rider.orders','status=complete') }}">{{ __('Completed Orders') }}<span class="badge badge-success">{{ $completed ?? \App\Riderlog::where('user_id',Auth::id())->where('status',1)->wherehas('completed')->count() }}</span>
            </a>
          </li>
        </ul>
      </div>
    </div>
  </div>


  <div class="col-12 mt-2">
    <div class="card">
      <div class="card-body">
        <div class="float-left">
          <form method="get" action="{{ route('rider.orders') }}">
           
            <div class="row">

              <div class="form-group ml-3">
                <label>{{ __('Starting Date') }}</label>
                <input type="date" name="start" class="form-control" required value="{{ $start ?? '' }}">
              </div>
              <div class="form-group ml-2">
               <label>{{ __('Starting Date') }}</label>
               <input type="date" name="end" class="form-control" required value="{{ $end ?? '' }}">
             </div>
           

             <div class="form-group mt-4">
              <button class="btn btn-primary btn-lg  ml-2 mt-1" type="submit">{{ __('Filter') }}</button>
            </div>
          </div>
        </form>
      </div>
      <div class="float-right">
        <form action="{{ route('rider.orders') }}">
          <div class="input-group mt-3 col-12">

            <input type="text" class="form-control" placeholder="Search By Order ID" required="" name="src" value="{{ $src ?? '' }}">
            <div class="input-group-append">                                            
              <button class="btn btn-primary" type="submit"><i class="fas fa-search"></i></button>
            </div>
          </div>
        </form>
      </div>


      <div class="table-responsive">
        <table class="table table-striped table-hover text-center table-borderless">
          <thead>
            <tr>
              <th>{{ __('Order ID') }}</th>
              <th>{{ __('Amount') }}</th>
              <th>{{ __('Status') }}</th>
              <th>{{ __('Time') }}</th>
              <th>{{ __('View') }}</th>
            </tr>
          </thead>
          <tbody>
            @foreach($orders as $key => $row )

           
            <tr>
              <td><a href="{{ route('rider.order.details',$row->order_id) }}">#{{ $row->order_id }}</a></td>
              
              <td>{{ strtoupper($currency->value) }} {{ $row->orders->total + $row->orders->shipping }}</td>
              
              
              <td>@if($row->status == 1) <span class="badge badge-success">{{ __('Accepted') }}</span> @elseif($row->status == 2) <span class="badge badge-primary"> {{ __('Pending') }} </span> @elseif($row->status == 3) <span class="badge badge-warning"> {{ __('Accepted') }} </span> @elseif($row->status == 0)  <span class="badge badge-danger"> {{ __('Cancelled') }} </span> @endif</td>
            
              <td>{{ $row->created_at->diffforHumans() }}</td>
              <td><a href="{{ route('rider.order.details',$row->order_id) }}" class="btn btn-primary"><i class="fas fa-eye"></i></a></td>
            </tr>
            @endforeach
          </tbody>

          <tfoot>
            <tr>
              <th>{{ __('Order ID') }}</th>
              <th>{{ __('Amount') }}</th>
              <th>{{ __('Status') }}</th>
              <th>{{ __('Time') }}</th>
              <th>{{ __('View') }}</th>
            </tr>
          </tfoot>
        </table>
        {{ $orders->links() }}
      </div>
    </div>
  </div>
</div>
</div>

@endsection