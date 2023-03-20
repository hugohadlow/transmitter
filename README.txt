README
One of the main problems with the world wide web is the conflation of identity with the network.
* You verify that information is from Encyclopedia Britannica by getting it from britannica.com (security certificates are a minor detail). If the information were signed, you could get it from anywhere and verify its authenticity. Multiple people could host the same information, and you could verify none of them had tampered with it.
* You send an email to user@example.com. The domain name is part of the identity -- this is bad design. If the user wants to change email provider, they have to create a wholly new identity, user@example.net. This might have already been claimed by someone else. Or the host could steal their identity. The solution is to use cryptographic identities: public_key@example.com.
* A Twitter account also uses the domain name for routing. Mastodon just reinvents email: user@mastodon1.com.

The solution is that cryptographic identities and digital signatures must become standard. Information can then be redistributed by anyone, and people can be confident it hasn't been tampered with.

I propose a standard JSON document format, the Transmitter message:
{
	"identity": "ABC123",
	"signature": "DEF456",
	"payload": "myPayload"
}

A message is not valid unless it is signed.

The default public key is 2048-bit RSA. The "identity" is the public key in base64.
The default signature is SHA256. The "signature" is the signature in base64.
The payload is any string.

Custom public key and signature algorithms are possible using additional JSON fields to specify them. This enables upgraded security in the future, for example SHA512.

Where possible, any special-purpose implementation should go inside the payload. For example, if it is desired to add a timestamp to a message, this should go inside the payload. This ensures that it is part of the signed payload. Another example: to sign a file, the file is distributed with a Message. The payload would be a JSON document containing a hash of the file.

//Implementation:

Models:
Message - contains identity, signature and payload.
Subscription - contains identity. This class is for use by the public REST api.

Stores:
KeyStore - to store our personal keypairs to disk.
MessageStore is for storing messages as flat files, grouped by subscription. You need to subscribe to your own messages to store them.

Tools:
KeyHelper generates keypairs, and also converts the public key to a base64 string.
Signer signs strings using a keypair.
Verifier verifies a Message.
Utils.Base64ToBase32 converts base64 strings to base32, for use as filenames.


