﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

class TestEncrypt {

    // --------------------- aes 
    // 参考: https://dotblogs.com.tw/jwpl102216/2016/10/23/120443
    public static string aesEncryptBase64(string SrcStr, string CryptoKey) {
        byte[] srcBts = Encoding.UTF8.GetBytes(SrcStr);
        byte[] dstBts = aesEncryptBase64(srcBts, CryptoKey);
        return dstBts != null ? Encoding.UTF8.GetString(dstBts) : "";
    }

    public static string aesDecryptBase64(string SrcStr, string CryptoKey) {
        byte[] srcBts = Encoding.UTF8.GetBytes(SrcStr);
        byte[] dstBts = aesDecryptBase64(srcBts, CryptoKey);
        return dstBts != null ? Encoding.UTF8.GetString(dstBts) : "";
    }

    public static byte[] aesEncryptBase64(byte[] dataByteArray, string CryptoKey) {
        byte[] encrypt = null;
        try {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(CryptoKey));
            byte[] iv = md5.ComputeHash(Encoding.UTF8.GetBytes(CryptoKey));
            aes.Key = key;
            aes.IV = iv;

            using(MemoryStream ms = new MemoryStream())
            using(CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write)) {
                cs.Write(dataByteArray, 0, dataByteArray.Length);
                cs.FlushFinalBlock();
                encrypt = ms.ToArray();
            }
        } catch (Exception e) {
            Console.WriteLine("aesEncryptBase64 Error!, msg: {0}", e.Message);
        }
        return encrypt;
    }

    public static byte[] aesDecryptBase64(byte[] dataByteArray, string CryptoKey) {
        byte[] decrypt = null;
        try {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(CryptoKey));
            byte[] iv = md5.ComputeHash(Encoding.UTF8.GetBytes(CryptoKey));
            aes.Key = key;
            aes.IV = iv;

            using(MemoryStream ms = new MemoryStream()) {
                using(CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write)) {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                    decrypt = ms.ToArray();
                }
            }
        } catch (Exception e) {
            Console.WriteLine("aesDecryptBase64 Error!, msg: {0}", e.Message);
        }
        return decrypt;
    }

    public static void test_aes() {
        string cryptoKey = "wolegequ";
        string srcDb = "E:/its_rummy/patch/android/release/1011_3_2_0.10.12.1/pack.db";
        string dstDb = "E:/its_rummy/patch/android/release/1011_3_2_0.10.12.1/pack_enc.db";

        byte[] bts = Utils.ReadAllBytesFromFile(srcDb);
        bts = aesEncryptBase64(bts, cryptoKey);
        Utils.WriteFile(dstDb, bts);
        Console.WriteLine("--- 写入 ok");

        bts = Utils.ReadAllBytesFromFile(dstDb);
        bts = aesDecryptBase64(bts, cryptoKey);
        string json = Utils.BytesToUTF8(bts);
        Console.WriteLine("--- 还原 ok, json: {0}", json);
    }

    public static void test_aesRead() {
        string cryptoKey = "U5RNM4beTo%@QmA";
        string srcDb = "C:/Users/wolegequ/Nox_share/ImageShare/platid.txt";

        byte[] bts = Utils.ReadAllBytesFromFile(srcDb);
        bts = aesDecryptBase64(bts, cryptoKey);
        string txt = Utils.BytesToUTF8(bts);
        Console.WriteLine("--- txt: {0}", txt);
    }

    public static void main() {
        // test_aes();
        test_aesRead();
    }
}