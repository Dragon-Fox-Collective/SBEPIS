# SBEPIS

SBEPIS is a first-person implementation of Homestuck's game SBURB. Our goal is to complete every hard task that other SBURB-sims haven't been able to do, like comprehensive AI and procedural alchemy.

Join our [Discord server](https://discord.gg/qHREQu7Zxm)!

## Cloning

```bash
git clone https://github.com/Dragon-Fox-Collective/SBEPIS.git -b act-2 --single-branch
cd SBEPIS
git submodule update --init --recursive
git submodule foreach --recursive git checkout main
```