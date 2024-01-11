import fastapi

api = fastapi.FastAPI()

@api.post("/transcribe", )
def transcribe(id: str, language: str = "auto", file: fastapi.UploadFile = fastapi.File(...)):
    return {"text": "demo string i am", "language": "yoda"}

def main():
    from uvicorn import run
    run(api, port=8000)

if __name__ == "__main__":
    main()
