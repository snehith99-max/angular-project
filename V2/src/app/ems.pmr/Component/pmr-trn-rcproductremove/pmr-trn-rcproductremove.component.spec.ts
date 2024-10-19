import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnRCproductremoveComponent } from './pmr-trn-rcproductremove.component';

describe('PmrTrnRCproductremoveComponent', () => {
  let component: PmrTrnRCproductremoveComponent;
  let fixture: ComponentFixture<PmrTrnRCproductremoveComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnRCproductremoveComponent]
    });
    fixture = TestBed.createComponent(PmrTrnRCproductremoveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
