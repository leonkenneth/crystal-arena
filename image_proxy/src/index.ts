import dotenv from 'dotenv';
import express, { Request, Response } from 'express';
import { generatePlaceholderImage, CardInfo } from './generatePlaceholder';

dotenv.config();
const app = express();

const PORT = process.env.PORT || 4000;
const ORIGIN_SERVER = process.env.IMGPROXY_ORIGIN_SERVER;

app.get('/images/cards/full/:serial(\\d+-\\d+[a-zA-Z]*)_:lang.jpg', async (req: Request, res: Response) => {
  const { serial, lang } = req.params;
  const cardInfo = req.query as unknown as CardInfo;
  const requestPath = `/images/cards/full/${serial}_${lang}.jpg`;
  console.log("GET", requestPath);
  
  if (!ORIGIN_SERVER) {
    res.contentType('image/jpeg');
    return res.send(await generatePlaceholderImage(serial, cardInfo));
  }

  const targetUrl = `${ORIGIN_SERVER}${requestPath}`;
  res.redirect(targetUrl);
});

app.listen(PORT, () => {
  console.log(`Image server running on port ${PORT}`);
  if (!ORIGIN_SERVER) {
    console.log('IMGPROXY_ORIGIN_SERVER not set, generating placeholder image');
  }
}); 