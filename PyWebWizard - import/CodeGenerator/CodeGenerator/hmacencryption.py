import hmac
import hashlib

def sign(data: bytes, key: bytes, algorithm=hashlib.sha256) -> bytes:
    assert len(key) >= algorithm().digest_size, (
        "Key must be at least as long as the digest size of the "
        "hashing algorithm"
    )
    return hmac.new(key, data, algorithm).digest()

def verify(signature: bytes, data: bytes, key: bytes, algorithm=hashlib.sha256) -> bytes:
    expected = sign(data, key, algorithm)
    return hmac.compare_digest(expected, signature)