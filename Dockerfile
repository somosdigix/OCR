FROM microsoft/dotnet:2.1-sdk-alpine AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-aspnetcore-runtime

RUN apt-get update && apt-get install -y python-dev tesseract-ocr g++ python-pip imagemagick
    
RUN pip install https://github.com/goulu/pdfminer/zipball/e6ad15af79a26c31f4e384d8427b375c93b03533#egg=pdfminer.six

RUN pip install docx2txt

WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Ocr.dll"]

# FROM microsoft/dotnet:2.1-sdk
# WORKDIR /app
# COPY . .

# RUN apt-get update && apt-get install -y python-dev tesseract-ocr g++ python-pip imagemagick