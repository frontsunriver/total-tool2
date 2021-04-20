@extends('layouts.backend.app')
@section('content')
@include('layouts.backend.partials.headersection',['title'=>'All Users'])
<div class="row">
  <div class="col-12 mt-2">
    <div class="card">
      <div class="card-body">
       <div class="float-left">
        <h5>{{ __('Total Customers') }} : {{ \App\User::where('role_id',3)->where('plan_id',$id)->count() }}</h5>
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
              <th>{{ __('User ID') }}</th>
              <th>{{ __('Avatar') }}</th>
              <th>{{ __('Name') }}</th>
              <th>{{ __('Email') }}</th>
              <th>{{ __('Purchased At') }}</th>
              <th>{{ __('View') }}</th>
            </tr>
          </thead>
          <tbody>
            @foreach($users as $row)
            <tr>
              <td><a href="{{ route('admin.vendor.show',$row->id) }}">{{ $row->id }}</a></td>
              <td><img src="{{ asset($row->avatar) }}" height="30"></td>
              <td><a href="{{ url('/store',$row->slug) }}" target="_blank">{{ $row->name }}</a></td>
              <td>{{ $row->email }}</td>
              <td>
                @if(!empty($row->saasmeta)) 
                @foreach($row->saasmeta as $saas)
                {{ $saas->updated_at->format('Y-F-d') }}
                @endforeach
                @endif
              </td>
              <td>
                <a href="{{ route('admin.vendor.show',$row->id) }}" class="btn btn-primary btn-sm"><i class="fas fa-eye"></i></a>
                <a href="{{ route('admin.plan.userplan',$row->id) }}" class="btn btn-success btn-sm"><i class="fas fa-edit"></i></a>
              </td>
            </tr>
            @endforeach
          </tbody>
          <tfoot>
            <tr>
              <th>{{ __('Vendor ID') }}</th>
              <th>{{ __('Avatar') }}</th>
              <th>{{ __('Name') }}</th>
              <th>{{ __('Email') }}</th>
              <th>{{ __('Purchased At') }}</th>
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

