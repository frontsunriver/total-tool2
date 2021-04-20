@extends('layouts.backend.app')
@section('content')
@include('layouts.backend.partials.headersection',['title'=>''])
<div class="row">
  <div class="col-12 mt-2">
    <div class="card">
      <div class="card-body">
       <div class="float-left">
        <h5>{{ __('Total Active Riders') }} : {{ \App\User::where('role_id',3)->where('status',$type)->count() }}</h5>
      </div>
      <div class="float-right">
        <form>
          <div class="input-group mb-2 col-12">

            <input type="text" class="form-control" placeholder="Search..." required="" name="src" autocomplete="off" value="{{ $src ?? '' }}">
            <select class="form-control" name="type">
              <option value="name">{{ __('Search By Name') }}</option>
              <option value="email">{{ __('Search By Email') }}</option>
              <option value="id">{{ __('Search By Id') }}</option>
            </select>
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
              <th>{{ __('Vendor ID') }}</th>
              <th>{{ __('Avatar') }}</th>
              <th>{{ __('Name') }}</th>
              <th>{{ __('Email') }}</th>
              
              @if($type=='pending')
              <th>{{ __('Email verified') }}</th>
              @endif
              <th>{{ __('Status') }}</th>
              <th>{{ __('Created at') }}</th>
              <th>{{ __('View') }}</th>
            </tr>
          </thead>
          <tbody>
            @foreach($users as $row)
            <tr>
              <td><a href="{{ route('admin.vendor.show',$row->id) }}">{{ $row->id }}</a></td>
              <td><img src="{{ asset($row->avatar) }}" height="30"></td>
              <td><a href="{{ route('admin.vendor.show',$row->id) }}" >{{ $row->name }}</a></td>
              <td>{{ $row->email }}</td>
              @if($type=='pending')
              <td>@if(!empty($row->email_verified_at)) <span class="badge badge-success">{{ __('Verified') }}</span> @else <span class="badge badge-danger">{{ __('Not verified') }}</span>  @endif</td>
              @endif
              <td>{{ $row->status }}</td>
              <td>{{ $row->created_at->format('Y-F-d') }}</td>
              <td><a href="{{ route('admin.vendor.show',$row->id) }}" class="btn btn-primary btn-sm"><i class="fas fa-eye"></i></a></td>
            </tr>
            @endforeach
          </tbody>
          <tfoot>
            <tr>
              <th>{{ __('Vendor ID') }}</th>
              <th>{{ __('Avatar') }}</th>
              <th>{{ __('Name') }}</th>
              <th>{{ __('Email') }}</th>
              @if($type=='pending')
              <th>{{ __('Email verified') }}</th>
              @endif
              <th>{{ __('Status') }}</th>

              <th>{{ __('Created at') }}</th>
              <th>{{ __('View') }}</th>
            </tr>
          </tfoot>
        </table>
        {{ $users->links() }}
      </div>
    </div>
  </div>
</div>
</div>
@endsection

