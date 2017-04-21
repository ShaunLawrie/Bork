import io.gatling.core.Predef._
import io.gatling.http.Predef._

        object flood {
          val uuid = System.getProperty("uuid")
          val httpConfigFlood = http
            .disableResponseChunksDiscarding
            .extraInfoExtractor((extraInfo) =>
              uuid ::
              extraInfo.response.statusCode.get.toString ::
              Option(extraInfo.response.header("Content-Length")).getOrElse("0") ::
              extraInfo.response.uri.get.toString ::
              extraInfo.request.getHeaders.toString ::
              extraInfo.response.headers.toString ::
              "" ::
              "" ::
              List()
            )
        }