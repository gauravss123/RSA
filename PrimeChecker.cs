/*
 * Input: n > 3, an odd integer to be tested for primality;
 * Input: k, a parameter that determines the accuracy of the test
 * Output: composite if n is composite, otherwise probably prime
 * write n − 1 as 2s·d with d odd by factoring powers of 2 from n − 1
 * WitnessLoop: repeat k times:
 *  pick a random integer a in the range [2, n − 2]
 *  x ← ad mod n
 *  if x = 1 or x = n − 1 then do next WitnessLoop
 *  repeat s − 1 times:
 *      x ← x2 mod n
 *      if x = 1 then return composite
 *      if x = n − 1 then do next WitnessLoop
 *  return composite
 *  return probably prime
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    int[] odd_number = new int[] { 1, 3, 5, 7, 9 };
    int  digits_no;
    int prime_bit = 32;
    int iterations = 64;
    int k;
    BigInteger  m = new BigInteger();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected BigInteger random_big(int size_bits,bool mode=true)
    {
        //find number of digits in a number which is size bits long
        //digits_no = Convert.ToInt16((size_bits-1)*Math.Log10(2) +1);
        digits_no = Text1.Value.Length;
        //prime_bit = size_bits;
        Random r = new Random();
        BigInteger value = new BigInteger();
        if (size_bits < 0)
            return BigInteger.Parse("-1");
        else
        {
         
            string big ="";
            while (big.Length < digits_no)
            {
                int rand = r.Next();
                big += rand.ToString();
            }
            int lenbf = big.Length;
            if (mode == true)
            {
                int rem = big.Length - (big.Length - digits_no) - 1;
                big = big.Remove(rem);

                big += odd_number[r.Next(4)].ToString();
            }
            else
            {
                int rem = big.Length - (big.Length - digits_no);
                big = big.Remove(rem);

            }

            
            //big += last_digit.ToString();

            value = BigInteger.Parse(big);
            BigInteger one = new BigInteger();
            one = BigInteger.MinusOne;
            one = BigInteger.Add(value,one);
            //TextBox1.Text +="     "+BigInteger.Remainder(one, BigInteger.Parse("2")).ToString()+"      ";
            int i = big.Length;
            return value;
            
        
        }

    }

    protected void decompose(BigInteger prime)
    {
        //code to find s,d in satisfying equation n-1 = 2^s*d
        int low = 1, upp = prime_bit, mid;
        BigInteger remainder = new BigInteger();
        BigInteger two = new BigInteger(2);
        BigInteger dividend = new BigInteger();
        dividend = BigInteger.Add(prime, BigInteger.MinusOne);
        do
        {
            mid = (low + upp) / 2;
            remainder = BigInteger.Remainder(dividend, BigInteger.Pow(two, mid));
            if (remainder.Equals(0))
                low = mid;
            else
                upp = mid;

        } while (low != upp - 1);
        k = low;
        m = BigInteger.Divide(dividend, BigInteger.Pow(two, k));
        TextBox2.Text = low.ToString() + " " + m.ToString();
    }


    protected bool is_witness(BigInteger possiblewitness, int exponent, BigInteger prime)
    {
        BigInteger two = new BigInteger(2);
        possiblewitness = BigInteger.ModPow(possiblewitness, m, prime);
        if (possiblewitness.Equals(1) || possiblewitness.Equals(BigInteger.Add(prime, BigInteger.MinusOne)))
            return false;
        while (exponent > 1)
        {
            possiblewitness = BigInteger.ModPow(possiblewitness, two, prime);
            if (possiblewitness.Equals(BigInteger.Add(prime, BigInteger.MinusOne)))
                return false;
            --exponent;
        }
        return true;
    }
    protected bool Prime_test(BigInteger prime)
    {
        decompose(prime);
        int iter = 1;
        BigInteger lowerlimit = new BigInteger(2);
        BigInteger upperlimit = new BigInteger();
        upperlimit = BigInteger.Subtract(prime,lowerlimit);
        BigInteger x = new BigInteger();
        bool result=true;
        while (iter++ <= iterations && result)
        { 
            BigInteger rand = new BigInteger();
            do
            {
                rand = random_big(prime_bit,false);
            }
            while (BigInteger.Compare(upperlimit, rand) < 0);
            if (is_witness(rand,k,prime))
                return false;
            //x = BigInteger.Remainder(rand,BigInteger.Add(prime,BigInteger.MinusOne));
 
          /*  x = BigInteger.ModPow(rand, m, prime);
            if (BigInteger.Compare(x, new BigInteger(1)) == 0 || BigInteger.Compare(x, BigInteger.Add(prime, BigInteger.MinusOne)) == 0)
                continue;
            else
            {
                int s = (int)k-1;
                while (s-- > 0)
                {

                    x = BigInteger.ModPow(x, lowerlimit, prime);
                    if (BigInteger.Compare(x, BigInteger.Add(prime, BigInteger.MinusOne)) == 0)
                    {
                        result = true;
                        break;
                    }
                    else if (x.Equals(1))
                        return false;

                }
                if (result==true)
                    continue;
                else
                    return false;
            }
                    */  
        }
        return true;


    }
       
    protected void Button1_Click(object sender, EventArgs e)
    {
        BigInteger p = new BigInteger();
        BigInteger q = new BigInteger();
        int size = Convert.ToInt16(Text1.Value.ToString());
        //p = random_big(Convert.ToInt16(Text1.Value.ToString())/2);
        /*do
        {
            p = random_big(size / 2);
        } while (!Prime_test(p));
        do
        {
            q = random_big(size / 2);
        } while (!Prime_test(q));*/

        TextBox1.Text =  Prime_test(new BigInteger(Convert.ToInt32(Text1.Value.ToString()))).ToString();
        //TextBox1.Text = ((a + b) / 2).ToString();  //p.ToString() + prim.ToString();
        
    }
}
