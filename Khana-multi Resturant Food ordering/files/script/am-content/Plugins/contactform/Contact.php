<?php 

namespace Amcoders\Plugin\contactform;
use Illuminate\Support\Facades\Mail;
use Amcoders\Plugin\contactform\Mail\ContactMail;

class Contact 
{
	public static function send($name,$email,$subject,$message)
	{
		$data = [
			'name' => $name,
			'email' => $email,
			'subject' => $subject,
			'message' => $message
		];
		Mail::to(env("MAIL_TO"))->send(new ContactMail($data));
		return response()->json(['Mail Sent Success']);
	}
}