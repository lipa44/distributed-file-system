import scala.compiletime.ops.int.+
import scala.concurrent.Future
import scala.util.*
import scala.language.*
import scala.language.experimental.macros
import scala.concurrent.ExecutionContext.Implicits.global
import scala.concurrent.duration._
import scala.concurrent.{Await, Future}
import scala.language.postfixOps

class ImportClassScala {
  // Pipe
  def PipeFunction(single: Int): Int = {
    single | sum(single, single) | multiply(single, single)
  }

  def sum(first: Int, second: Int): Int = {
    first + second
  }

  def multiply(first: Int, second: Int): Int = {
    first * second
  }

  // ComputationExpression
  def ComputationExpression(): Unit = {
    val f: Future[String] = Future {
      Thread.sleep(2000)
      "future value"
    }

    val f2 = f map { s =>
      println("OK!")
      println("OK!")
    }

    Await.ready(f2, 5 seconds)
    println("exit")
  }

  def aboba(): Unit = {
    println("aboba")
  }
}

// Union
class Shape(centerX: Int, centerY: Int):
  case class Square(side: Int, centerX: Int, centerY: Int) extends Shape(centerY, centerX)

  case class Rectangle(length: Int, height: Int, centerX: Int, centerY: Int) extends Shape(centerX, centerY)

  case class Circle(radius: Int, centerX: Int, centerY: Int) extends Shape(centerX, centerY)
