import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnDirectpoAddLatestComponent } from './pmr-trn-directpo-add-latest.component';

describe('PmrTrnDirectpoAddLatestComponent', () => {
  let component: PmrTrnDirectpoAddLatestComponent;
  let fixture: ComponentFixture<PmrTrnDirectpoAddLatestComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnDirectpoAddLatestComponent]
    });
    fixture = TestBed.createComponent(PmrTrnDirectpoAddLatestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
