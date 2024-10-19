import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstWhatsappproducteditComponent } from './smr-mst-whatsappproductedit.component';

describe('SmrMstWhatsappproducteditComponent', () => {
  let component: SmrMstWhatsappproducteditComponent;
  let fixture: ComponentFixture<SmrMstWhatsappproducteditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstWhatsappproducteditComponent]
    });
    fixture = TestBed.createComponent(SmrMstWhatsappproducteditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
