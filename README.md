# UnityEditorPackageJsonCreator

自作ライブラリを PackageManager で利用できるようにするためには、package.json が必要になり
毎回以前のものを参考にして作成するのが面倒になったので、簡単に作成できるようなツールを作りました

連想配列のシリアライズが出来ないので、ロード処理は自前で作成する必要があったため、現状はロード処理は対応していません

<img src="https://cdn-ak.f.st-hatena.com/images/fotolife/h/hacchi_man/20201123/20201123023708.png" width="500">


# 使い方
SetPath より保存先のディレクトリを選択

その後必要な箇所を記述
※この際に空の場合はその項目は出力されません

Name, Version, UnityVersion には必要な文字以外は消すようになっています

Nameは [a-b] . - のみ利用できます

編集後は Save を押すと、最初に指定したディレクトリに package.json が出力されます

```json
{
  "name": "yorozu.tool.package-json-creator",
  "version": "0.0.1",
  "displayName": "PackageJsonCreator",
  "description": "package.json を作成するツール",
  "author": {
    "name": "Yorozu",
    "email": "hatch.tech.eng@gmail.com",
    "url": "https://github.com/yayorozu"
  },
  "keywords": [
    "tool",
    "json"
  ]
}
```
