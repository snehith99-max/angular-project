import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrRptVendorreportViewComponent } from './pmr-rpt-vendorreport-view.component';

describe('PmrRptVendorreportViewComponent', () => {
  let component: PmrRptVendorreportViewComponent;
  let fixture: ComponentFixture<PmrRptVendorreportViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrRptVendorreportViewComponent]
    });
    fixture = TestBed.createComponent(PmrRptVendorreportViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
