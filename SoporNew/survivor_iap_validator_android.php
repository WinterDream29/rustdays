<?php
function verify_market_in_app($signed_data, $signature, $public_key_base64)
{
	$key =	"-----BEGIN PUBLIC KEY-----\n".
		chunk_split($public_key_base64, 64,"\n").
		'-----END PUBLIC KEY-----';   
	//using PHP to create an RSA key
	$key = openssl_get_publickey($key);
	//$signature should be in binary format, but it comes as BASE64. 
	//So, I'll convert it.
	$signature = base64_decode($signature);   
	//using PHP's native support to verify the signature
	$result = openssl_verify(
			$signed_data,
			$signature,
			$key,
			OPENSSL_ALGO_SHA1);
	if (0 === $result) 
	{
		return 0;
	}
	else if (1 !== $result)
	{
		return 0;
	}
	else 
	{
		return 1;
	}
} 

$base64EncodedPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAmqBp4hnt3yxeWcJgBUAvkoMUyhMBWdSAxagL9jUST+4DV1FvmMugBDkmaCrCvdpA39iAbMrCr+GgGCNF8XmDtje1PxVzmPAI3O4vAcTSsy3C2iLSyRcY9ukV74kQIdrskY08I8mt+KFLgwTbzH8zv9jaAiAmrbUAbRv1Tj6kJ7PB9G9rYAcvMyAmY+oz7fcj/sqgjqwrIyoa/ye46d/WeHiTZcuj+pkYGR+oLZ6mvwC9YnAplQIXMqJLY/62AA3ZFUoEHF1+U1FTU/FizxMu3cyb2SSQGpLTt8jaQHwOGxJNJUky1cw0EFuwXJc3dKz9PjLhrJo9u1B/xxhyEx5kvwIDAQAB";
$signedData =  $_POST['data'];
$signature =  $_POST['signature']; 

echo verify_market_in_app($signedData, $signature, $base64EncodedPublicKey);
?>