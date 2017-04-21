import io.gatling.core.Predef._
import io.gatling.http.Predef._
import scala.util.Random

// Home page actions
object Bork {

    // Submit a new bork
    val borks = csv("borks.csv").random
    val newBork = feed(borks)
        .doIfOrElse(List("snuffles","snowball").contains("${username}")){
        exec(
            http("Submit New Bork")
            .post("/bork")
                .body(StringBody("""{"content":"🐶 Where are my testicles, Summer? 🔫😡💢"}"""))
                .asJSON
            .check(substring("""userName":"${username}","content":""")))  
        } {
        exec(
            http("Submit New Bork")
            .post("/bork")
                .body(StringBody("""{"content":"${bork}"}"""))
                .asJSON
            .check(substring("""userName":"${username}","content":""")))
        }

    // Rebork someone elses bork
    val reBork = exec(
        http("Poll Top Borks")
        .get("/bork/topborks")
        .check(substring("<div class=\"bork-content\">"))
        .check(regex("""id="bork-([0-9]+)" class="rebork-link""")
            .findAll.exists.saveAs("original_bork_ids")))
        .exec(
        session =>
            session.set("random_rebork_id",
            Random.shuffle(session("original_bork_ids").as[Seq[Int]]).head))
        .exec(
        http("Submit ReBork")
            .post("/rebork/${random_rebork_id}")
            .body(StringBody("")).asJSON
            .check(status.is(201))
            .check(substring("""originalBorkId":${random_rebork_id},""")))

    // View the about page
    val about = exec(
        http("View About")
        .get("/About")
            .check(substring("Example Web App for Performance Testing Testing")))

}