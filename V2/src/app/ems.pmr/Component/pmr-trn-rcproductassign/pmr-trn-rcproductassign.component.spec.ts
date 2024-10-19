import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnRCproductassignComponent } from './pmr-trn-rcproductassign.component';

describe('PmrTrnRCproductassignComponent', () => {
  let component: PmrTrnRCproductassignComponent;
  let fixture: ComponentFixture<PmrTrnRCproductassignComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnRCproductassignComponent]
    });
    fixture = TestBed.createComponent(PmrTrnRCproductassignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
