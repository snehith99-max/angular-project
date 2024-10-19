import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstWaproductpriceupdateComponent } from './smr-mst-waproductpriceupdate.component';

describe('SmrMstWaproductpriceupdateComponent', () => {
  let component: SmrMstWaproductpriceupdateComponent;
  let fixture: ComponentFixture<SmrMstWaproductpriceupdateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstWaproductpriceupdateComponent]
    });
    fixture = TestBed.createComponent(SmrMstWaproductpriceupdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
